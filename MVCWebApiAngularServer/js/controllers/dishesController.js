App.controller("dishesController", function ($scope, dishesconfigService, $state, $rootScope, FileUploadService) {

    var vm = this;

    vm.newEntry = {
        dishname:'',
        imagesrc: '',
        TimeToRun:'',
        script:''
    };

    vm.AppConfig = {
        MotorLength: 100
    };

    $scope.dishes = [];
    $scope.hideform = true;
    $scope.editform = false;
    $scope.createform = false;
    
    dishesconfigService.getAllDishes().then(function (result) {
        
        var d = JSON.parse(result.data);       
        for (var i = 0 ; i < d.length; i++)
        {
            var miniute = d[i].TimeToRun.Minutes;
            var seconds = d[i].TimeToRun.Seconds;
            d[i].TimeToRun = miniute + ":" + seconds;
        }
        $scope.dishes = d;

    }).catch(function (result) {
        console.log(result);
    })
     
    dishesconfigService.GetAppConfig().then(function (result) {
       
        var d = JSON.parse(result.data);
        vm.AppConfig = d[0];

    }).catch(function (result) {
        console.log(result);
    })

    $scope.createDish = function ()
    {
        $scope.hideform = false;
        $scope.createform = true;
        $scope.editform = false;
    }

    $scope.deleteDish = function(id)
    {
        dishesconfigService.deleteDishById(id).then(function (result) {
            var d = JSON.parse(result.data);
            for (var i = 0 ; i < d.length; i++) {
                var miniute = d[i].TimeToRun.Minutes;
                var seconds = d[i].TimeToRun.Seconds;
                d[i].TimeToRun = miniute + ":" + seconds;
            }
            $scope.dishes = d;


            $scope.hideform = true;
            $scope.editform = false;
        }).catch(function (result) {
            alert(result.data);
        })
    }

    $scope.editDish = function (id)
    {
       
        dishesconfigService.getDishById(id).then(function (result) {            
            var d = JSON.parse(result.data);
            vm.Entry = d[0];

            var miniute = d[0].TimeToRun.Minutes;
            var seconds = d[0].TimeToRun.Seconds;
            vm.Entry.TimeToRun = miniute + ":" + seconds;


            $scope.hideform = false;
            $scope.editform = true;
        }).catch(function (result) {
            alert(result.data);
        })
    }
    $scope.UpdateDish = function()
    { 
        dishesconfigService.UpdateDish(vm.Entry).then(function (result) {
             
            var d = JSON.parse(result.data);
            for (var i = 0 ; i < d.length; i++) {
                var miniute = d[i].TimeToRun.Minutes;
                var seconds = d[i].TimeToRun.Seconds;
                d[i].TimeToRun = miniute + ":" + seconds;
            }
            $scope.dishes = d;


            $scope.hideform = true;
            $scope.editform = false;

            dishesconfigService.SetDishImageId(vm.Entry.Id).then(function (result) {

                FileUploadService.UploadFile($scope.SelectedFileForUpload, vm.Entry.Id).then(function (d) {


                });
            });

        }).catch(function (result) {
            alert(result.data);
        })
    }
    $scope.CreateNewEntry = function()
    {
         
        dishesconfigService.CreateNewDish(vm.newEntry).then(function (result) {

            $scope.dishes = JSON.parse(result.data);
            $scope.hideform = true;
            console.log(result.data);
            var id = parseInt(result.data);

            dishesconfigService.SetDishImageId(id).then(function (result) {

                FileUploadService.UploadFile($scope.SelectedFileForUpload, id).then(function (d) {
                    

                    dishesconfigService.getAllDishes().then(function (result) {

                        var d = JSON.parse(result.data);
                        for (var i = 0 ; i < d.length; i++) {
                            var miniute = d[i].TimeToRun.Minutes;
                            var seconds = d[i].TimeToRun.Seconds;
                            d[i].TimeToRun = miniute + ":" + seconds;
                        }
                        $scope.dishes = d;
                    }).catch(function (result) {
                        console.log(result);
                    })


                }, function (e) {
                    alert('failed to upload');
                });
            }).catch(function (result) {
                alert(result.data);
            });

        }).catch(function (result) {
            alert(result.data);
        })
    }

    $scope.UpdateAppConfig = function () {     
        dishesconfigService.UpdateAppConfig(vm.AppConfig).then(function (result) {

            dishesconfigService.GetAppConfig().then(function (result) {

                var d = JSON.parse(result.data);
                vm.AppConfig = d[0];

            }).catch(function (result) {
                console.log(result);
            })

 
        }).catch(function (result) { 
            alert(result.data);
        })
    }

    //File Select event 
    $scope.selectFileforUpload = function (file) {
    
        $scope.SelectedFileForUpload = file[0];
    }

    $scope.UploadNow = function(id)
    {
        dishesconfigService.SetDishImageId(id).then(function (result) {
            FileUploadService.UploadFile($scope.SelectedFileForUpload, id).then(function (d) {
                
                dishesconfigService.getAllDishes().then(function (result) {

                    var d = JSON.parse(result.data);
                    for (var i = 0 ; i < d.length; i++) {
                        var miniute = d[i].TimeToRun.Minutes;
                        var seconds = d[i].TimeToRun.Seconds;
                        d[i].TimeToRun = miniute + ":" + seconds;
                    }
                    $scope.dishes = d;
                }).catch(function (result) {
                    console.log(result);
                })

            }, function (e) {
                console.log(e)
            });
        }).catch(function (result) {
            alert('Error uploading image');
        })

    }
   
});