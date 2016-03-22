
angular.module('App.directives.contactCard', []).
directive("contactCard", function () {
    //$("#exampleArray").sevenSeg({ digits: 5, value: 12.35 });
    return {
        restrict: "E",
        link: function (scope, el, atts) {
            el.click(function(){
                alert("Eli Arad");
            })
        }
    }
});