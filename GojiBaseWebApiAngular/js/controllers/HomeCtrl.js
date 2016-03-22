App.controller("HomeCtrl", function ($scope, HomeService, $msgbox, $rootScope, $state, dishesconfigService)
{


    var vm = $scope;
    $scope.clock = "0:0";

    HomeService.IsRunning().then(function (result) {
        if (result.data == false)
        {
            $scope.running = 0;
            dishesconfigService.getAllDishes().then(function (result) {
                $scope.DishesDataBase = JSON.parse(result.data);
            }).catch(function (result) {
                console.log(result);
            })
        } else {
            $scope.running = 1;
            $scope.currentimagedishname = sessionStorage.getItem('currentimagedishname');
            $scope.currentimagedishsrc = sessionStorage.getItem('currentimagedishsrc');
        }
    }).catch(function (result) {
        alert('Error detecting is running');
    })   
    /*
    $scope.DishesDataBase = [
        {
            Name: 'gigot',
            ImageSrc: '/images/2016_03_carrousel_699x408_gigot.png',
            Script: 'name:potato;move:40;wait:10;move:20;wait:1;move:40;wait:4;move:10'
        },
         {
             Name: 'cheeseburger',
             ImageSrc: '/images/2016_03_carrousel_699x408_mini_cheeseburger.png',
             Script: 'name: cheeseburger; move:40; wait: 10'
         },
         {
             Name: 'oeuf patissier',
             ImageSrc: '/images/2016_03_carrousel_699x408_oeuf_patissier.png',
             Script: 'name: oeuf patissier; move:40; wait: 10'
         }
    ];
    */
    $scope.ShowStartWindow = function()
    {
        $scope.running = 0;
    }

    $rootScope.$on("Finished", function () {
         
        $scope.running = 2;
        sessionStorage.setItem('currentimagedishname', '');
        sessionStorage.setItem('currentimagedishsrc', '');
        $scope.$digest();
        $scope.$apply();
         
    });

    $scope.ShowAlert = function(text)
    {
        if (text == 'Finished') {
            $rootScope.$broadcast('Finished');
        }        
    }

    $scope.$on("UpdateClock", function (event, message) {
        
        $scope.clock = message;
        console.log($scope.clock);
        $scope.$digest();
        $scope.$apply();
    });

    $scope.UpdateClock = function (time) {

        //this.$emit("UpdateClock", time);
        $rootScope.$broadcast('UpdateClock', time);
    }

    $scope.OpenConfig = function()
    {
        $state.go('/', {}, {
            reload: true
        });
    }

    $scope.RunDish = function(item)
    {

        var txt;
        var r = confirm("Do you want to start?");
        if (r == false) {
            return;
        }

        $scope.currentimagedishname = item.Name;
        $scope.currentimagedishsrc = item.ImageSrc;

        HomeService.RunDish(item).then(function (result) {

            sessionStorage.setItem('currentimagedishname', item.Name);
            sessionStorage.setItem('currentimagedishsrc', item.ImageSrc);

            $scope.running = 1;
        }).catch(function (result) {
            console.log(result);
            alert(result.data);
        })

    }    
});

 
 
