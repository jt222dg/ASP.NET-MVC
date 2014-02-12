$(function () {
    var $endCoordinates = $('#end-coordinates'),
		$mapCanvas = $('.map-canvas'),
		headerHeight = $('.header').outerHeight();

    function showTab(target, link) {
        $('.sidebar-nav a').removeClass('active').filter(link).addClass('active');
        $('.sidebar [class|=tab]').hide().filter(target).show();
    }

    LnuMap.init(function (locations, types) {
        var i = 0,
			html = '',
			term;

        $('#q').Autocomplete({
            locationList: locations,
            update: true
        });

        $('#end').Autocomplete({
            locationList: locations
        }).on('keyup', function (e) {
            if (e.keyCode !== 13) {
                $endCoordinates.val('');
            }
        });

        if (location.hash) {
            term = decodeURIComponent(location.hash.split('#')[1]);

            $('#q').val(term).trigger('keyup');
            $('.location-list li').each(function () {
                if ($(this).text() === term) {
                    $(this).trigger('click');

                    return false;
                }
            });
        }
    });

    LnuMap.doMap('.map-canvas', '.directions-panel');
    LnuMap.setTypes([1, 3, 5, 8, 9, 24, 28, 49]);

    $(window).on('load resize', function (e) {
        var center = e.type === 'load';

        $mapCanvas.height($(window).height() - headerHeight - 15);

        LnuMap.refresh(center);
    });

    $('.directions-form').on('submit', function (e) {
        var $_this = $(this),
			end = $_this.find('#end').val();

        if ($endCoordinates.val()) {
            end = $endCoordinates.val();
        }

        e.preventDefault();

        LnuMap.doDirection($_this.find('#start').val(), end, google.maps.TravelMode.DRIVING);
    });

    $('.search-form').on('submit', function (e) {
        e.preventDefault();

        $('.location-list').fadeIn();
    });

    $(document).on('locationSelected', function (e, location, update) {
        var hash;

        if (!location) {
            return;
        }

        hash = '#' + encodeURIComponent(location.fullName);

        LnuMap.setLocations([location], update, true);

        if (update) {
            if (history.pushState) {
                history.pushState(null, null, hash);
            } else {
                location.hash = hash;
            }
        } else {
            $endCoordinates.val(location.latitude + ', ' + location.longitude);
        }
    });

    $('.sidebar-nav').on('click', 'a', function (e) {
        var target = $(this).attr('href');

        e.preventDefault();

        showTab(target, this);
    });

    $('#filters').on('change', 'input[type=checkbox]', function () {
        var types = [];

        $(this).parents('.filter-list').find('input[type=checkbox]').each(function () {
            var $_this = $(this);

            if ($_this.is(':checked')) {
                if ($_this.val() === 'area') {
                    types.push('area');
                } else {
                    types.push(parseInt($_this.val(), 10));
                }
            }
        });

        LnuMap.doSearchForFilter(types);
    });

    $('body').on('click', '.find-direction', function (e) {
        var $_this = $(this);

        e.preventDefault();

        $('#start').trigger('focus');
        $('#end').val($_this.data('name'));
        $endCoordinates.val($_this.data('location'));

        showTab('#directions', '.directions-link');
    });

    $('.shortcuts').on('click', 'a', function (e) {
        var id = $(this).data('id'),
			city = LnuMap.cities[id],
			position = new google.maps.LatLng(city.latitude, city.longitude);

        e.preventDefault();

        LnuMap.currentMap.setCenter(position);
        LnuMap.currentMap.setZoom(14);
    });
});