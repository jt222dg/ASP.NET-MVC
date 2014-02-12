function removeNestedForm(element, container, deleteElement) {
    $container = $(element).parents(container);
    $input_nodes = $container.children().val("null");
    $container.find(deleteElement).val('True');
    $container.hide();
}

function addNestedForm(container, counter, ticks, content) {
    var nextIndex = $(container).children('.locationName').length;
    var pattern = new RegExp(ticks, "gi");
    content = content.replace(pattern, nextIndex);
    $(container).append(content);
}

$(function () {
    $('#Area_city_id').change(function () {
        doInput(this, '#HasChosenCity', 'Välj stad', 'area')
    });
    $('#City_id').change(function () {
        doInput(this, '#HasChosenCity', 'Välj stad', 'location')
    });
    $('#Location_area_id').change(function () {
        doInput(this, '#HasChosenArea', 'Välj område', 'location')
    });
    function doInput(dropdown, name , notChosen, controllerName) {
        
        var input = $(name);
        var selected = $('#' + $(dropdown).attr('id') + " option:selected").text();
        if (selected == notChosen) {
            input.attr('value', false);
        }
        else{
            input.attr('value', true);
        }
        var input2 = $('#HasChosenArea');
        if (controllerName == "location" && name == "#HasChosenCity") {
            input2.attr('value', false);
        }
        var formId = '#' + controllerName + '_form';
        $(formId).submit();
    }
});
