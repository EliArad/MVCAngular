App.factory("HomeService", function($http){
    var _HomeData = [];

    var IsRunning = function()
    {        
        return $http.get("/api/main/IsRunning");
    }

    var RunDish = function(data)
    {        
        return $http.post("/api/main/RunDish", data );
    }

    var _getHomeService = function ()
    {
       
        $http.get("/api/main/Index")
            .then(function (results) {
                var js = JSON.parse(results.data);
                for (i = 0; i < js.length; i++) {
                    var value = new Date(parseInt(js[i].StartMeasure.replace("/Date(", "").replace(")/", ""), 10));
                    var value1 = new Date(parseInt(js[i].StoppedMeasure.replace("/Date(", "").replace(")/", ""), 10));
                    //alert(value);
                    var nd = value.getFullYear() + "/" + (value.getMonth() + 1) + "/" + (value.getUTCDate()) + " " + value.getHours() + ":" + value.getMinutes() + ":" + value.getSeconds();
                    var nd1 = value1.getFullYear() + "/" + (value1.getMonth() + 1) + "/" + (value1.getUTCDate()) + " " + value1.getHours() + ":" + value1.getMinutes() + ":" + value1.getSeconds();
                    js[i].StartMeasure = nd;
                    js[i].StoppedMeasure = nd1;
                }
                angular.copy(js, _HomeData); //this is the preferred; instead of $scope.movies = result.data               
            }, function(results){
                //Error
                alert("Error in get movies 1");
            })
    }

    var Alert = function () {
        alert ("11");
    }

    var _StartWatson = function (id) {

        $http.get("/StartWatson?id=" + id)
            .then(function(results){
                //Success
                //angular.copy(results.data, _HomeData); //this is the preferred; instead of $scope.movies = result.data
                //var x = JSON.stringify(_HomeData);
            }, function(results){
                //Error
                alert ("Error in Start Watson");
            })

    }

    var _StopWatson = function (id) {

        $http.get("/StopWatson?id=" + id)
            .then(function(results){
                //Success
                //angular.copy(results.data, _HomeData); //this is the preferred; instead of $scope.movies = result.data
                //var x = JSON.stringify(_HomeData);
            }, function(results){
                //Error
                alert ("Error in Stop Watson");
            })

    }

    var _HeatWatson = function (id) {

        $http.get("/HeatWatson?id=" + id)
            .then(function(results){
                //Success
                //angular.copy(results.data, _HomeData); //this is the preferred; instead of $scope.movies = result.data
                //var x = JSON.stringify(_HomeData);
            }, function(results){
                //Error
                alert ("Error in Heat Watson");
            })

    }

    var _GetNumberOfFiles = function (id) {

        $http.get("/GetNumberOfFiles?id=" + id)
            .then(function(results){
                //Success
                //angular.copy(results.data, _HomeData); //this is the preferred; instead of $scope.movies = result.data
                //var x = JSON.stringify(_HomeData);
            }, function(results){
                //Error
                alert ("Error in GetNumberOfFiles");
            })

    }
     
    return{
        HomeData: _HomeData,
        getHomeService: _getHomeService,
        StartWatson: _StartWatson,
        StopWatson: _StopWatson,
        HeatWatson:_HeatWatson,
        GetNumberOfFiles: _GetNumberOfFiles,
        IsRunning: IsRunning,
        RunDish: RunDish
    };
});
