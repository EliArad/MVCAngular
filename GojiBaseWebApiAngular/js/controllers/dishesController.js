App.controller("dishesController", function ($scope, dishesconfigService, $state, $rootScope) {

    var vm = this;

    vm.newEntry = {
        dishname:'',
        imagesrc:'',
        script:''
    };

    $scope.dishes = [];
    $scope.hideform = true;
    $scope.editform = false;
    
    dishesconfigService.getAllDishes().then(function (result) {
        
        $scope.dishes = JSON.parse(result.data);
    }).catch(function (result) {
        console.log(result);
    })

    $scope.createDish = function ()
    {
        $scope.hideform = false;
    }

    $scope.deleteDish = function(id)
    {
        dishesconfigService.deleteDishById(id).then(function (result) {
            $scope.dishes = JSON.parse(result.data);
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
            $scope.hideform = true;
            $scope.editform = true;
        }).catch(function (result) {
            alert(result.data);
        })
    }
    $scope.UpdateDish = function()
    {

        dishesconfigService.UpdateDish(vm.Entry).then(function (result) {

            $scope.dishes = JSON.parse(result.data);
            $scope.hideform = true;
            $scope.editform = false;
        }).catch(function (result) {
            alert(result.data);
        })
    }
    $scope.CreateNewEntry = function()
    {
         
        dishesconfigService.CreateNewDish(vm.newEntry).then(function (result) {

            $scope.dishes = JSON.parse(result.data);
            $scope.hideform = true;
        }).catch(function (result) {
            alert(result.data);
        })

    }
    
});