(function () {
	var LnuMap = {
		areas: [],
		cities: [],
		currentMap: null,
		defaultCenter: new google.maps.LatLng(56.73509, 15.48602),
		directionsDisplay: null,
		directionsService: null,
		languageCode: 'sv',
		languageNames: {
			en: 'English',
			sv: 'Swedish'
		},
		lastWindow: null,
		locations: [],
		locationTypes: [],
		markers: [],
		setupCallback: null,
		usedTypes: [],
		zoomLevel: 17,

		addInfo: function (location, marker, position, open) {
			var self = this,
				content;

			content = '<div class="info-window">';
			content += '<h2>' + location.mainName + '</h2>';

			if (location.name !== location.mainName && location.area) {
				content += '<h3>' + location.name + '</h3>';
			}

			if (location.type) {
				content += location.type + '<br>';
			}

			if (location.floor !== undefined) {
				content += (self.languageCode === 'sv' ? 'Våningsplan: ' : 'Floor: ') + location.floor + '<br><br>';
			}

			if (location.area) {
				content += location.area.name + '<br>';
			}

			content += '<p class="alignright"><button href="#" class="find-direction" data-location="' + location.latitude + ', ' + location.longitude + '" data-name="' + location.fullName + '">' + (self.languageCode === 'sv' ? 'Hitta hit' : 'Get here') + '</button></p>';
			content += '</div>';

			if (open) {
				self.showWindow(content, position, open);
			}

			google.maps.event.addListener(marker, 'click', function () {
				self.currentMap.panTo(position);

				if (self.currentMap.getZoom() < self.zoomLevel) {
					self.currentMap.setZoom(self.zoomLevel);
				}

				self.showWindow(content, position);
			});
		},

		clear: function () {
			for (i = 0; i < this.markers.length; i++) {
				this.markers[i].setMap(null);
			}

			this.markers = [];
		},

		doMap: function (mapSelector, directionsSelector, options) {
			var defaults = {
				center: this.defaultCenter,
				mapTypeId: google.maps.MapTypeId.HYBRID,
				zoom: 8
			};

			options = $.extend(defaults, options);

			this.currentMap = new google.maps.Map(document.querySelector(mapSelector), options);
			this.directionsDisplay = new google.maps.DirectionsRenderer();
			this.directionsService = new google.maps.DirectionsService();

			if (directionsSelector) {
				this.directionsDisplay.setMap(this.currentMap);
				this.directionsDisplay.setPanel(document.querySelector(directionsSelector));
			}
		},

		doSearchForFilter: function (typeIDs) {
			var i,
				locations,
				location;

			this.clear();

			locations = $.grep(this.locations, function (element) {
				if ($.inArray(element.typeID, typeIDs) !== -1) {
					return true;
				}

				return false;
			});

			for (i = 0; i < locations.length; i++) {
				if (this.languageCode === 'en') {
					if (locations[i].area) {
						locations[i].area.name = locations[i].area.english.name;
					}

					locations[i].fullName = locations[i].english.fullName;
					locations[i].mainName = locations[i].english.mainName;
					locations[i].name = locations[i].english.name;
				} else {
					if (locations[i].area) {
						locations[i].area.name = locations[i].area.swedish.name;
					}

					locations[i].fullName = locations[i].swedish.fullName;
					locations[i].mainName = locations[i].swedish.mainName;
					locations[i].name = locations[i].swedish.name;
				}
			}

			console.log(locations);

			this.setLocations(locations);
		},

		doDirection: function (start, end, mode) {
			var self = this,
				request = {
					destination: end,
					origin: start,
					travelMode: mode
				};

			self.directionsService.route(request, function (result, status) {
				if (status === google.maps.DirectionsStatus.OK) {
					self.directionsDisplay.setDirections(result);
				}
			});
		},

		init: function (callback) {
			var self = this,
				languageName;

			self.setupCallback = callback;

			if ($('html').attr('lang')) {
				self.languageCode = $('html').attr('lang');
				languageName = self.languageNames[self.languageCode];
			}

			$.ajaxSetup({
				cache: false,
				dataType: 'json'
			});

			$.ajax({
                url: '/api/cities',
				success: function (data) {
					var city,
						current;

					for (city in data) {
						if (data.hasOwnProperty(city)) {
							current = data[city];

							self.cities[current.City_ID] = {
								latitude: current.Latitude,
								longitude: current.Longitude,
								name: current.Name
							};
						}
					}

					$.ajax({
                        url: '/api/areas/',
						success: function (data) {
							var area,
								areas = [],
								obj,
								current;

							for (area in data) {
								if (data.hasOwnProperty(area)) {
									current = data[area];

									obj = {
										city: self.cities[current.City_ID],
										english: {
											fullName: current.Name['English'],
											mainName: '',
											name: current.Name['English']
										},
										id: current.ID,
										latitude: current.Latitude,
										longitude: current.Longitude,
										swedish: {
											fullName: current.Name['Swedish'],
											mainName: '',
											name: current.Name['Swedish']
										},
										typeIcon: 'byggnad.png',
										typeID: 'area'
									};

									self.areas[current.ID] = obj;

									areas.push(obj);
								}
							}

							$.ajax({
								url: '/api/locations/',
								success: function (data) {
									var location,
										current,
										i,
										cityName,
										names,
										area;

									for (location in data) {
										names = {
											english: [],
											swedish: []
										};

										if (data.hasOwnProperty(location)) {
											current = data[location];

											for (i = 0; i < current.Names.length; i++) {
												if (current.Names[i]) {
													if (current.Names[i]['English'] !== current.English_Main_Name) {
														names.english.push(current.Names[i]['English']);
													} else if (current.Names[i]['Swedish'] !== current.Swedish_Main_Name) {
														names.swedish.push(current.Names[i]['Swedish']);
													}
												}
											}

											area = self.areas[current.Area_ID];
											city = area.city;
											cityName = ', ' + city.name;

											self.locations.push({
												area: area,
												city: city,
												english: {
													fullName: (names.english.length ? names.english.join(', ') : current.English_Main_Name) + ', ' + area.english.name + cityName,
													mainName: current.English_Main_Name,
													name: names.english
												},
												floor: current.Floor_Number,
												id: current.Location_ID,
												latitude: current.Latitude,
												longitude: current.Longitude,
												swedish: {
													fullName: (names.swedish.length ? names.swedish.join(', ') : current.Swedish_Main_Name) + ', ' + area.swedish.name + cityName,
													mainName: current.Swedish_Main_Name,
													name: names.swedish
												},
												type: '',
												typeIcon: '',
												typeID: current.Location_Type_ID
											});
										}
									}

									self.setLocationTypes();

									for (i = 0; i < areas.length; i++) {
										areas[i].english.name = areas[i].english.name;
										areas[i].english.mainName = areas[i].english.name + ', ' + areas[i].city.name;

										areas[i].swedish.name = areas[i].swedish.name;
										areas[i].swedish.mainName = areas[i].swedish.name + ', ' + areas[i].city.name;

										self.locations.push(areas[i]);
									}

									if (self.setupCallback) {
										self.setupCallback.call(self, self.locations, self.locationTypes);
									}
								}
							});
						}
					});
				}
			});

			$.ajax({
                url: '/api/locationtypes/',
				success: function (data) {
					var i,
						current,
						type,
						html = '';

					html += '<li><label><img src="/Content/icons/byggnad.png" alt=""> <input type="checkbox" value="area">' + (self.languageCode === 'sv' ? 'Byggnad' : 'Building' ) + '</label></li>';
					for (i = 0; i < data.length; i++) {
						current = data[i];

						type = {
							icon: current.Icon_URL,
							id: current.Location_Type_ID,
							name: current.Name[languageName],
							url: current.Icon_URL
						};

						self.locationTypes[current.Location_Type_ID] = type;

						if ($.inArray(current.Location_Type_ID, self.usedTypes) !== -1) {
							html += '<li><label><img src="/Content/icons/' + type.icon + '" alt=""> <input type="checkbox" value="' + type.id + '">' + type.name + '</label></li>';
						}
					}

					$('.filter-list').html(html);
				}
			});
		},

		refresh: function (center) {
			google.maps.event.trigger(this.currentMap, 'resize');

			if (center) {
				this.currentMap.setCenter(this.defaultCenter);
			}
		},

		setLocations: function (locations, openWindow, clear) {
			var i,
				position,
				location,
				size = new google.maps.Size(21, 33);

			if (clear) {
				this.clear();
			}

			for (i = 0; i < locations.length; i++) {
				location = locations[i];
				position = new google.maps.LatLng(location.latitude, location.longitude);

				marker = new google.maps.Marker({
					icon: new google.maps.MarkerImage('/Content/icons/' + location.typeIcon, null, null, null, size),
					map: this.currentMap,
					position: position
				});

				if (locations.length === 1) {
					this.currentMap.setCenter(position);
					this.currentMap.setZoom(this.zoomLevel);

					this.addInfo(location, marker, position, true);
				} else {
					this.addInfo(location, marker, position, openWindow);
				}

				this.markers.push(marker);
			}
		},

		setLocationTypes: function () {
			var i,
				current;

			for (i = 0; i < this.locations.length; i++) {
				current = this.locations[i];

				this.locations[i].type = this.locationTypes[current.typeID].name;
				this.locations[i].typeIcon = this.locationTypes[current.typeID].url;
			}
		},

		setTypes: function (types) {
			this.usedTypes = types;
		},

		showWindow: function (content, position) {
			if (this.lastWindow) {
				this.lastWindow.close();
			}

			this.lastWindow = new google.maps.InfoWindow({
				content: content,
				position: position
			});

			this.lastWindow.open(this.currentMap);
		}
	};

	window.LnuMap = LnuMap;
}());

(function ($) {
	'use strict';

	$.fn.Autocomplete = function (options) {
		var defaults = {
				locationList: [],
				maxResults: 10,
				update: false
			};

		options = $.extend(defaults, options);

		function hideList($list) {
			$list.fadeOut();
		}

		function selectLocation($input, $item, $list) {
			var location = $item.data('location');

			$(document).trigger('locationSelected', [location, options.update]);

			if (location) {
				$input.val(location.fullName);
			}

			hideList($list);
		}

		return this.each(function () {
			var $input = $(this),
				$item,
				$list = $input.after('<ul class="location-list">').next('.location-list');

			$input.on('keyup', function (e) {
				var location,
					matchingLocations = [],
					val = $input.val().toLowerCase(),
					i,
					maxLength = options.maxResults,
					html = $(),
					object;

				if (val.length >= 3) {
					$list.fadeIn();
				} else {
					$list.fadeOut(function () {
						$(this).html('');
					});
				}

				switch (e.keyCode) {
					case 13:
						selectLocation($input, $item, $list);

						$input.trigger('blur');

						e.preventDefault();

						break;
					case 27:
						hideList($list);

						break;
					case 38:
					case 40:
						e.preventDefault();

						break;
					default:
						for (location in options.locationList) {
							location = options.locationList[location];

							if (location.english.fullName.toLowerCase().indexOf(val) !== -1) {
								if (location.area) {
									location.area.name = location.area.english.name;
								}

								location.fullName = location.english.fullName;
								location.mainName = location.english.mainName;
								location.name = location.english.name;
							} else if (location.swedish.fullName.toLowerCase().indexOf(val) !== -1) {
								if (location.area) {
									location.area.name = location.area.swedish.name;
								}

								location.fullName = location.swedish.fullName;
								location.mainName = location.swedish.mainName;
								location.name = location.swedish.name;
							} else {
								continue;
							}

							matchingLocations.push(location);
						}

						if (matchingLocations.length < maxLength) {
							maxLength = matchingLocations.length;
						}

						for (i = 0; i < maxLength; i++) {
							location = matchingLocations[i];

							object = $('<li>').data('location', location).html('<a href="#">' + location.fullName + '</a>');

							html = html.add(object);
						}

						$item = $list.html('').append(html).find('li:first');
				}

				$item.addClass('location-active');
			}).on('keydown', function (e) {
				var $newItem = [],
					update = false;

				$list.find('li').removeClass('location-active');

				switch (e.keyCode) {
					case 38:
						$newItem = $item.prev();

						update = true;

						break;
					case 40:
						$newItem = $item.next();

						update = true;

						break;
				}

				if ($newItem.length) {
					$item = $newItem;
				}

				if (update) {
					$item.addClass('location-active');

					e.preventDefault();
				}
			}).on('click', function (e) {
				e.stopPropagation();
			});

			$(document).on('click', function () {
				hideList($list);
			});

			$list.on('click', 'li', function () {
				selectLocation($input, $(this), $list);

				return false;
			});
		});
	};
}(jQuery));

/*! Copyright (c) 2011 Brandon Aaron (http://brandonaaron.net)
 * Licensed under the MIT License (LICENSE.txt).
 *
 * Thanks to: http://adomas.org/javascript-mouse-wheel/ for some pointers.
 * Thanks to: Mathias Bank(http://www.mathias-bank.de) for a scope bug fix.
 * Thanks to: Seamus Leahy for adding deltaX and deltaY
 *
 * Version: 3.0.6
 *
 * Requires: 1.2.2+
 */
(function(d){function e(a){var b=a||window.event,c=[].slice.call(arguments,1),f=0,e=0,g=0,a=d.event.fix(b);a.type="mousewheel";b.wheelDelta&&(f=b.wheelDelta/120);b.detail&&(f=-b.detail/3);g=f;void 0!==b.axis&&b.axis===b.HORIZONTAL_AXIS&&(g=0,e=-1*f);void 0!==b.wheelDeltaY&&(g=b.wheelDeltaY/120);void 0!==b.wheelDeltaX&&(e=-1*b.wheelDeltaX/120);c.unshift(a,f,e,g);return(d.event.dispatch||d.event.handle).apply(this,c)}var c=["DOMMouseScroll","mousewheel"];if(d.event.fixHooks)for(var h=c.length;h;)d.event.fixHooks[c[--h]]= d.event.mouseHooks;d.event.special.mousewheel={setup:function(){if(this.addEventListener)for(var a=c.length;a;)this.addEventListener(c[--a],e,!1);else this.onmousewheel=e},teardown:function(){if(this.removeEventListener)for(var a=c.length;a;)this.removeEventListener(c[--a],e,!1);else this.onmousewheel=null}};d.fn.extend({mousewheel:function(a){return a?this.bind("mousewheel",a):this.trigger("mousewheel")},unmousewheel:function(a){return this.unbind("mousewheel",a)}})})(jQuery);

/*
 * jScrollPane - v2.0.0beta11 - 2012-04-23
 * http://jscrollpane.kelvinluck.com/
 *
 * Copyright (c) 2010 Kelvin Luck
 * Dual licensed under the MIT and GPL licenses.
 */
(function(d,ka,q){d.fn.jScrollPane=function(z){function ca(b,z){function da(a){var c,f,r,o,v,n=!1,k=!1;e=a;if(g===q)o=b.scrollTop(),v=b.scrollLeft(),b.css({overflow:"hidden",padding:0}),j=b.innerWidth()+L,i=b.innerHeight(),b.width(j),g=d('<div class="jspPane" />').css("padding",la).append(b.children()),h=d('<div class="jspContainer" />').css({width:j+"px",height:i+"px"}).append(g).appendTo(b);else{b.css("width","");n=e.stickToBottom&&sa();k=e.stickToRight&&ta();if(r=b.innerWidth()+L!=j||b.outerHeight()!= i)j=b.innerWidth()+L,i=b.innerHeight(),h.css({width:j+"px",height:i+"px"});if(!r&&ma==s&&g.outerHeight()==p){b.width(j);return}ma=s;g.css("width","");b.width(j);h.find(">.jspVerticalBar,>.jspHorizontalBar").remove().end()}g.css("overflow","auto");s=a.contentWidth?a.contentWidth:g[0].scrollWidth;p=g[0].scrollHeight;g.css("overflow","");ea=s/j;V=p/i;x=1<V;w=1<ea;if(!w&&!x)b.removeClass("jspScrollable"),g.css({top:0,width:h.width()-L}),h.unbind(fa),g.find(":input,a").unbind("focus.jsp"),b.attr("tabindex", "-1").removeAttr("tabindex").unbind("keydown.jsp keypress.jsp"),na();else{b.addClass("jspScrollable");if(a=e.maintainPosition&&(l||m))c=D(),f=E();ca();ua();va();a&&(N(k?s-j:c,!1),F(n?p-i:f,!1));wa();xa();ya();e.enableKeyboardNavigation&&za();e.clickOnTrack&&Aa();Ba();e.hijackInternalLinks&&Ca()}e.autoReinitialise&&!O?O=setInterval(function(){da(e)},e.autoReinitialiseDelay):!e.autoReinitialise&&O&&clearInterval(O);o&&b.scrollTop(0)&&F(o,!1);v&&b.scrollLeft(0)&&N(v,!1);b.trigger("jsp-initialised",[w|| x])}function ca(){x&&(h.append(d('<div class="jspVerticalBar" />').append(d('<div class="jspCap jspCapTop" />'),d('<div class="jspTrack" />').append(d('<div class="jspDrag" />').append(d('<div class="jspDragTop" />'),d('<div class="jspDragBottom" />'))),d('<div class="jspCap jspCapBottom" />'))),W=h.find(">.jspVerticalBar"),A=W.find(">.jspTrack"),t=A.find(">.jspDrag"),e.showArrows&&(R=d('<a class="jspArrow jspArrowUp" />').bind("mousedown.jsp",G(0,-1)).bind("click.jsp",P),S=d('<a class="jspArrow jspArrowDown" />').bind("mousedown.jsp", G(0,1)).bind("click.jsp",P),e.arrowScrollOnHover&&(R.bind("mouseover.jsp",G(0,-1,R)),S.bind("mouseover.jsp",G(0,1,S))),oa(A,e.verticalArrowPositions,R,S)),H=i,h.find(">.jspVerticalBar>.jspCap:visible,>.jspVerticalBar>.jspArrow").each(function(){H=H-d(this).outerHeight()}),t.hover(function(){t.addClass("jspHover")},function(){t.removeClass("jspHover")}).bind("mousedown.jsp",function(a){d("html").bind("dragstart.jsp selectstart.jsp",P);t.addClass("jspActive");var c=a.pageY-t.position().top;d("html").bind("mousemove.jsp", function(a){M(a.pageY-c,false)}).bind("mouseup.jsp mouseleave.jsp",pa);return false}),qa())}function qa(){A.height(H+"px");l=0;ga=e.verticalGutter+A.outerWidth();g.width(j-ga-L);try{0===W.position().left&&g.css("margin-left",ga+"px")}catch(a){}}function ua(){w&&(h.append(d('<div class="jspHorizontalBar" />').append(d('<div class="jspCap jspCapLeft" />'),d('<div class="jspTrack" />').append(d('<div class="jspDrag" />').append(d('<div class="jspDragLeft" />'),d('<div class="jspDragRight" />'))),d('<div class="jspCap jspCapRight" />'))), X=h.find(">.jspHorizontalBar"),B=X.find(">.jspTrack"),u=B.find(">.jspDrag"),e.showArrows&&(T=d('<a class="jspArrow jspArrowLeft" />').bind("mousedown.jsp",G(-1,0)).bind("click.jsp",P),U=d('<a class="jspArrow jspArrowRight" />').bind("mousedown.jsp",G(1,0)).bind("click.jsp",P),e.arrowScrollOnHover&&(T.bind("mouseover.jsp",G(-1,0,T)),U.bind("mouseover.jsp",G(1,0,U))),oa(B,e.horizontalArrowPositions,T,U)),u.hover(function(){u.addClass("jspHover")},function(){u.removeClass("jspHover")}).bind("mousedown.jsp", function(a){d("html").bind("dragstart.jsp selectstart.jsp",P);u.addClass("jspActive");var c=a.pageX-u.position().left;d("html").bind("mousemove.jsp",function(a){Q(a.pageX-c,false)}).bind("mouseup.jsp mouseleave.jsp",pa);return false}),y=h.innerWidth(),ra())}function ra(){h.find(">.jspHorizontalBar>.jspCap:visible,>.jspHorizontalBar>.jspArrow").each(function(){y-=d(this).outerWidth()});B.width(y+"px");m=0}function va(){if(w&&x){var a=B.outerHeight(),c=A.outerWidth();H-=a;d(X).find(">.jspCap:visible,>.jspArrow").each(function(){y+= d(this).outerWidth()});y-=c;i-=c;j-=a;B.parent().append(d('<div class="jspCorner" />').css("width",a+"px"));qa();ra()}w&&g.width(h.outerWidth()-L+"px");p=g.outerHeight();V=p/i;w&&(I=Math.ceil(1/ea*y),I>e.horizontalDragMaxWidth?I=e.horizontalDragMaxWidth:I<e.horizontalDragMinWidth&&(I=e.horizontalDragMinWidth),u.width(I+"px"),J=y-I,ha(m));x&&(K=Math.ceil(1/V*H),K>e.verticalDragMaxHeight?K=e.verticalDragMaxHeight:K<e.verticalDragMinHeight&&(K=e.verticalDragMinHeight),t.height(K+"px"),C=H-K,ia(l))}function oa(a, c,f,d){var e="before",b="after";"os"==c&&(c=/Mac/.test(navigator.platform)?"after":"split");c==e?b=c:c==b&&(e=c,c=f,f=d,d=c);a[e](f)[b](d)}function G(a,c,f){return function(){Da(a,c,this,f);this.blur();return!1}}function Da(a,c,f,r){var f=d(f).addClass("jspActive"),b,v,n=!0,h=function(){0!==a&&k.scrollByX(a*e.arrowButtonSpeed);0!==c&&k.scrollByY(c*e.arrowButtonSpeed);v=setTimeout(h,n?e.initialDelay:e.arrowRepeatFreq);n=!1};h();b=r?"mouseout.jsp":"mouseup.jsp";r=r||d("html");r.bind(b,function(){f.removeClass("jspActive"); v&&clearTimeout(v);v=null;r.unbind(b)})}function Aa(){na();x&&A.bind("mousedown.jsp",function(a){if(a.originalTarget===q||a.originalTarget==a.currentTarget){var c=d(this),f=c.offset(),b=a.pageY-f.top-l,o,v=!0,n=function(){var f=c.offset(),f=a.pageY-f.top-K/2,d=i*e.scrollPagePercent,g=C*d/(p-i);if(0>b)l-g>f?k.scrollByY(-d):M(f);else if(0<b)l+g<f?k.scrollByY(d):M(f);else{h();return}o=setTimeout(n,v?e.initialDelay:e.trackClickRepeatFreq);v=!1},h=function(){o&&clearTimeout(o);o=null;d(document).unbind("mouseup.jsp", h)};n();d(document).bind("mouseup.jsp",h);return!1}});w&&B.bind("mousedown.jsp",function(a){if(a.originalTarget===q||a.originalTarget==a.currentTarget){var c=d(this),f=c.offset(),b=a.pageX-f.left-m,o,h=!0,n=function(){var f=c.offset(),f=a.pageX-f.left-I/2,d=j*e.scrollPagePercent,i=J*d/(s-j);if(0>b)m-i>f?k.scrollByX(-d):Q(f);else if(0<b)m+i<f?k.scrollByX(d):Q(f);else{g();return}o=setTimeout(n,h?e.initialDelay:e.trackClickRepeatFreq);h=!1},g=function(){o&&clearTimeout(o);o=null;d(document).unbind("mouseup.jsp", g)};n();d(document).bind("mouseup.jsp",g);return!1}})}function na(){B&&B.unbind("mousedown.jsp");A&&A.unbind("mousedown.jsp")}function pa(){d("html").unbind("dragstart.jsp selectstart.jsp mousemove.jsp mouseup.jsp mouseleave.jsp");t&&t.removeClass("jspActive");u&&u.removeClass("jspActive")}function M(a,c){x&&((0>a?a=0:a>C&&(a=C),c===q&&(c=e.animateScroll),c)?k.animate(t,"top",a,ia):(t.css("top",a),ia(a)))}function ia(a){a===q&&(a=t.position().top);h.scrollTop(0);l=a;var c=0===l,f=l==C,a=-(a/C)*(p- i);if(Y!=c||Z!=f)Y=c,Z=f,b.trigger("jsp-arrow-change",[Y,Z,$,aa]);e.showArrows&&(R[c?"addClass":"removeClass"]("jspDisabled"),S[f?"addClass":"removeClass"]("jspDisabled"));g.css("top",a);b.trigger("jsp-scroll-y",[-a,c,f]).trigger("scroll")}function Q(a,c){w&&((0>a?a=0:a>J&&(a=J),c===q&&(c=e.animateScroll),c)?k.animate(u,"left",a,ha):(u.css("left",a),ha(a)))}function ha(a){a===q&&(a=u.position().left);h.scrollTop(0);m=a;var c=0===m,f=m==J,a=-(a/J)*(s-j);if($!=c||aa!=f)$=c,aa=f,b.trigger("jsp-arrow-change", [Y,Z,$,aa]);e.showArrows&&(T[c?"addClass":"removeClass"]("jspDisabled"),U[f?"addClass":"removeClass"]("jspDisabled"));g.css("left",a);b.trigger("jsp-scroll-x",[-a,c,f]).trigger("scroll")}function F(a,c){M(a/(p-i)*C,c)}function N(a,c){Q(a/(s-j)*J,c)}function ba(a,c,f){var b,o,g=0,n=0,k,l,m;try{b=d(a)}catch(p){return}o=b.outerHeight();a=b.outerWidth();h.scrollTop(0);for(h.scrollLeft(0);!b.is(".jspPane");)if(g+=b.position().top,n+=b.position().left,b=b.offsetParent(),/^body|html$/i.test(b[0].nodeName))return; b=E();k=b+i;g<b||c?l=g-e.verticalGutter:g+o>k&&(l=g-i+o+e.verticalGutter);l&&F(l,f);g=D();l=g+j;n<g||c?m=n-e.horizontalGutter:n+a>l&&(m=n-j+a+e.horizontalGutter);m&&N(m,f)}function D(){return-g.position().left}function E(){return-g.position().top}function sa(){var a=p-i;return 20<a&&10>a-E()}function ta(){var a=s-j;return 20<a&&10>a-D()}function xa(){h.unbind(fa).bind(fa,function(a,c,f,d){a=m;c=l;k.scrollBy(f*e.mouseWheelSpeed,-d*e.mouseWheelSpeed,!1);return a==m&&c==l})}function P(){return!1}function wa(){g.find(":input,a").unbind("focus.jsp").bind("focus.jsp", function(a){ba(a.target,!1)})}function za(){function a(){var a=m,d=l;switch(c){case 40:k.scrollByY(e.keyboardSpeed,!1);break;case 38:k.scrollByY(-e.keyboardSpeed,!1);break;case 34:case 32:k.scrollByY(i*e.scrollPagePercent,!1);break;case 33:k.scrollByY(-i*e.scrollPagePercent,!1);break;case 39:k.scrollByX(e.keyboardSpeed,!1);break;case 37:k.scrollByX(-e.keyboardSpeed,!1)}return f=a!=m||d!=l}var c,f,r=[];w&&r.push(X[0]);x&&r.push(W[0]);g.focus(function(){b.focus()});b.attr("tabindex",0).unbind("keydown.jsp keypress.jsp").bind("keydown.jsp", function(b){if(!(b.target!==this&&(!r.length||!d(b.target).closest(r).length))){var e=m,g=l;switch(b.keyCode){case 40:case 38:case 34:case 32:case 33:case 39:case 37:c=b.keyCode;a();break;case 35:F(p-i);c=null;break;case 36:F(0),c=null}f=b.keyCode==c&&e!=m||g!=l;return!f}}).bind("keypress.jsp",function(b){b.keyCode==c&&a();return!f});e.hideFocus?(b.css("outline","none"),"hideFocus"in h[0]&&b.attr("hideFocus",!0)):(b.css("outline",""),"hideFocus"in h[0]&&b.attr("hideFocus",!1))}function Ba(){if(location.hash&& 1<location.hash.length){var a,c,f=escape(location.hash.substr(1));try{a=d("#"+f+', a[name="'+f+'"]')}catch(b){return}a.length&&g.find(f)&&(0===h.scrollTop()?c=setInterval(function(){0<h.scrollTop()&&(ba(a,!0),d(document).scrollTop(h.position().top),clearInterval(c))},50):(ba(a,!0),d(document).scrollTop(h.position().top)))}}function Ca(){d(document.body).data("jspHijack")||(d(document.body).data("jspHijack",!0),d(document.body).delegate("a[href*=#]","click",function(a){var c=this.href.substr(0,this.href.indexOf("#")), b=location.href,e;-1!==location.href.indexOf("#")&&(b=location.href.substr(0,location.href.indexOf("#")));if(c===b){c=escape(this.href.substr(this.href.indexOf("#")+1));e;try{e=d("#"+c+', a[name="'+c+'"]')}catch(g){return}e.length&&(c=e.closest(".jspScrollable"),b=c.data("jsp"),b.scrollToElement(e,!0),c[0].scrollIntoView&&(b=d(ka).scrollTop(),e=e.offset().top,(e<b||e>b+d(ka).height())&&c[0].scrollIntoView()),a.preventDefault())}}))}function ya(){var a,c,b,d,e,g=!1;h.unbind("touchstart.jsp touchmove.jsp touchend.jsp click.jsp-touchclick").bind("touchstart.jsp", function(h){h=h.originalEvent.touches[0];a=D();c=E();b=h.pageX;d=h.pageY;e=!1;g=!0}).bind("touchmove.jsp",function(h){if(g){var h=h.originalEvent.touches[0],i=m,j=l;k.scrollTo(a+b-h.pageX,c+d-h.pageY);e=e||5<Math.abs(b-h.pageX)||5<Math.abs(d-h.pageY);return i==m&&j==l}}).bind("touchend.jsp",function(){g=!1}).bind("click.jsp-touchclick",function(){if(e)return e=!1})}var e,k=this,g,j,i,h,s,p,ea,V,x,w,t,C,l,u,J,m,W,A,ga,H,K,R,S,X,B,y,I,T,U,O,la,L,ma,Y=!0,$=!0,Z=!1,aa=!1,ja=b.clone(!1,!1).empty(),fa= d.fn.mwheelIntent?"mwheelIntent.jsp":"mousewheel.jsp";la=b.css("paddingTop")+" "+b.css("paddingRight")+" "+b.css("paddingBottom")+" "+b.css("paddingLeft");L=(parseInt(b.css("paddingLeft"),10)||0)+(parseInt(b.css("paddingRight"),10)||0);d.extend(k,{reinitialise:function(a){a=d.extend({},e,a);da(a)},scrollToElement:function(a,c,b){ba(a,c,b)},scrollTo:function(a,c,b){N(a,b);F(c,b)},scrollToX:function(a,c){N(a,c)},scrollToY:function(a,c){F(a,c)},scrollToPercentX:function(a,c){N(a*(s-j),c)},scrollToPercentY:function(a, c){F(a*(p-i),c)},scrollBy:function(a,c,b){k.scrollByX(a,b);k.scrollByY(c,b)},scrollByX:function(a,c){var b=(D()+Math[0>a?"floor":"ceil"](a))/(s-j);Q(b*J,c)},scrollByY:function(a,c){var b=(E()+Math[0>a?"floor":"ceil"](a))/(p-i);M(b*C,c)},positionDragX:function(a,c){Q(a,c)},positionDragY:function(a,c){M(a,c)},animate:function(a,c,b,d){var g={};g[c]=b;a.animate(g,{duration:e.animateDuration,easing:e.animateEase,queue:!1,step:d})},getContentPositionX:function(){return D()},getContentPositionY:function(){return E()}, getContentWidth:function(){return s},getContentHeight:function(){return p},getPercentScrolledX:function(){return D()/(s-j)},getPercentScrolledY:function(){return E()/(p-i)},getIsScrollableH:function(){return w},getIsScrollableV:function(){return x},getContentPane:function(){return g},scrollToBottom:function(a){M(C,a)},hijackInternalLinks:d.noop,destroy:function(){var a=E(),c=D();b.removeClass("jspScrollable").unbind(".jsp");b.replaceWith(ja.append(g.children()));ja.scrollTop(a);ja.scrollLeft(c);O&& clearInterval(O)}});da(z)}z=d.extend({},d.fn.jScrollPane.defaults,z);d.each(["mouseWheelSpeed","arrowButtonSpeed","trackClickSpeed","keyboardSpeed"],function(){z[this]=z[this]||z.speed});return this.each(function(){var b=d(this),q=b.data("jsp");q?q.reinitialise(z):(q=new ca(b,z),b.data("jsp",q))})};d.fn.jScrollPane.defaults={showArrows:!1,maintainPosition:!0,stickToBottom:!1,stickToRight:!1,clickOnTrack:!0,autoReinitialise:!1,autoReinitialiseDelay:500,verticalDragMinHeight:0,verticalDragMaxHeight:99999, horizontalDragMinWidth:0,horizontalDragMaxWidth:99999,contentWidth:q,animateScroll:!1,animateDuration:300,animateEase:"linear",hijackInternalLinks:!1,verticalGutter:4,horizontalGutter:4,mouseWheelSpeed:0,arrowButtonSpeed:0,arrowRepeatFreq:50,arrowScrollOnHover:!1,trackClickSpeed:0,trackClickRepeatFreq:70,verticalArrowPositions:"split",horizontalArrowPositions:"split",enableKeyboardNavigation:!0,hideFocus:!1,keyboardSpeed:0,initialDelay:300,speed:30,scrollPagePercent:0.8}})(jQuery,this);

// jQuery.XDomainRequest.js
// Author: Jason Moon - @JSONMOON
// IE8+
if(!jQuery.support.cors&&window.XDomainRequest){var httpRegEx=/^https?:\/\//i,getOrPostRegEx=/^get|post$/i,sameSchemeRegEx=RegExp("^"+location.protocol,"i"),jsonRegEx=/\/json/i,xmlRegEx=/\/xml/i;jQuery.ajaxTransport("text html xml json",function(a,b){if(a.crossDomain&&a.async&&getOrPostRegEx.test(a.type)&&httpRegEx.test(b.url)&&sameSchemeRegEx.test(b.url)){var d=null,e=(b.dataType||"").toLowerCase();return{send:function(c,f){d=new XDomainRequest,/^\d+$/.test(b.timeout)&&(d.timeout=b.timeout),d.ontimeout=function(){f(500,"timeout")},d.onload=function(){var a="Content-Length: "+d.responseText.length+"\r\nContent-Type: "+d.contentType,b={code:200,message:"success"},c={text:d.responseText};try{if("json"===e||"text"!==e&&jsonRegEx.test(d.contentType))try{c.json=$.parseJSON(d.responseText)}catch(g){b.code=500,b.message="parseerror"}else if("xml"===e||"text"!==e&&xmlRegEx.test(d.contentType)){var h=new ActiveXObject("Microsoft.XMLDOM");h.async=!1;try{h.loadXML(d.responseText)}catch(g){h=void 0}if(!h||!h.documentElement||h.getElementsByTagName("parsererror").length)throw b.code=500,b.message="parseerror","Invalid XML: "+d.responseText;c.xml=h}}catch(i){throw i}finally{f(b.code,b.message,c,a)}},d.onerror=function(){f(500,"error",{text:d.responseText})};var g=b.data&&$.param(b.data)||"";d.open(a.type,a.url),d.send(g)},abort:function(){d&&d.abort()}}}})}