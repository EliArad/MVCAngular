App.controller("HomeCtrl", function ($scope, HomeService, $msgbox, $rootScope, $state, dishesconfigService, $cookies)
{


    var vm = $scope;
    $scope.clock = "0:0";

    var expireDate = new Date();
    expireDate.setDate(expireDate.getDate() + 10);
   

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
            $scope.currentimagedishname = $cookies.get('currentimagedishname');
            $scope.currentimagedishsrc = $cookies.get('currentimagedishsrc');

            console.log($scope.currentimagedishsrc);
            console.log($scope.currentimagedishname);
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

        $cookies.put('currentimagedishname', '', { 'expires': expireDate });
        $cookies.put('currentimagedishsrc', '', { 'expires': expireDate });
        $scope.$digest();
        $scope.$apply();
         
    });

    $scope.ShowAlert = function(text)
    {
        if (text == 'Finished') {
            $rootScope.$broadcast('Finished');
        }
        if (text == 'motoControl_Attach')
        {

        }
    }


    
    $scope.$on("MotorUpdateValue", function (event, value) {

        $scope.motorValue = value;
        console.log(value);
        $scope.$digest();
        $scope.$apply();
    });

    $scope.$on("UpdateClock", function (event, message) {
        
        $scope.clock = message;
        $scope.$digest();
        $scope.$apply();
    });

    
    $scope.MotorUpdateValue = function (value) {

        //this.$emit("UpdateClock", time);
        $rootScope.$broadcast('MotorUpdateValue', value);
    }

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

    $scope.StopCooking = function()
    {
        var txt;
        var r = confirm("Do you want to stop?");
        if (r == false) {
            return;
        }

        HomeService.StopCooking().then(function (result) {
            $scope.running = 0;
        }).catch(function (result) {
            console.log(result);
            alert(result.data);
        })
    }

   

    $scope.RunDish = function(item)
    {
        HomeService.isMotorConnected().then(function (result) {
            if (result.data == "Connected") {
                var txt;
                var r = confirm("Do you want to start?");
                if (r == false) {
                    return;
                }
                 

                HomeService.RunDish(item).then(function (result) {

                    $scope.currentimagedishname = item.Name;
                    $scope.currentimagedishsrc = item.ImageSrc;

                    $cookies.put('currentimagedishname', item.Name, { 'expires': expireDate });
                    $cookies.put('currentimagedishsrc', item.ImageSrc, { 'expires': expireDate });

                    console.log(item);

                    $scope.running = 1;
                }).catch(function (result) {
                    console.log(result);
                    alert(result.data);
                })
            } else {
                alert("Motor is disconncted");
            }
        }).catch(function (result) {

        })      
    }    
});

 
 
