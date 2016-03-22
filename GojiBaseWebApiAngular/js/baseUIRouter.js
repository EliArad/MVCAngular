
var App = angular.module('App', ['ui.router', 'ui.bootstrap', 'App.directives.sevensegmentComponent', 'msgbox']);

  
    App.config(function($stateProvider, $urlRouterProvider ) {

   $urlRouterProvider.otherwise('/');

   $stateProvider
       .state('/', {
           url: '/',
           templateUrl: 'js/views/index.html',
           controller: 'HomeCtrl'
       }).state('dishconfig', {
           url: '/dishconfig',
           templateUrl: 'js/views/dishesconfig.html',
       });
        
});
  


angular.module('App.directives.sevensegmentComponent', []).
directive("sevensegmentComponent", function () {
    return {
        restrict: "AE",
        require: 'ngModel',     
        
        link: function (scope, el, atts, ngModel)
        {
            if (!ngModel)
                return; // do nothing if no ng-model
            el.sevenSeg({
                digits: atts.digits,
                value: "0",
                colorOn: atts.colorOn,
                colorOff: atts.colorOff,
            });
                         
            ngModel.$render = function () {
                
                if (ngModel.$viewValue != "undefined" && ngModel.$viewValue.id != "undefined") {
                    //console.log(atts);
                    if (atts.index == ngModel.$viewValue.id)
                        el.sevenSeg({ value: ngModel.$viewValue.value });
                }
            }             
        }
    }
});
 

 