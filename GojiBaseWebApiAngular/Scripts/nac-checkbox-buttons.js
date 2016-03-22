/// <reference path="jquery-1.9.1.js" />

$(document).ready(function () {
    // find all checkboxes and radio buttons
    // and move to inside the label
    $(".show-checkbox input[type=checkbox], .show-checkbox input[type=radio]")
        .each(function () {
            var selector = "label[for=" + this.id + "]";
            var label = $(this).siblings(selector);
            $(this).appendTo(label);
        });

    // set class for any preselected checkboxes or radio buttons
    $(".show-checkbox input[type=checkbox]:checked, .show-checkbox input[type=radio]:checked")
        .parent("label")
        .addClass("checkbox-checked");

    // add/remove class to label for checkbox if checked on change
    $(".show-checkbox input[type=checkbox]").change(function () {
        var checked = this.checked;
        if (checked === true)
            $(this).parent().addClass("checkbox-checked");
        else
            $(this).parent().removeClass("checkbox-checked");
    });

    $(".show-checkbox input[type=radio]").change(function () {
        // remove class from all labels in radio list
        $(this).parents().children("label").removeClass("checkbox-checked");

        // check newly selected radio
        var checked = this.checked;
        if (checked === true)
            $(this).parent().addClass("checkbox-checked");
    });
});
