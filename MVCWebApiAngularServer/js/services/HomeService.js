App.factory("HomeService", function($http){

    var IsRunning = function()
    {        
        return $http.get("/api/main/IsRunning");
    }

    var RunDish = function(data)
    {        
        return $http.post("/api/main/RunDish", data );
    }

    var StopCooking = function () {
        return $http.get("/api/main/StopCooking");
    }
    var isMotorConnected = function () {
        return $http.get("/api/main/isMotorConnected");
    }
     
     
    return{
        IsRunning: IsRunning,
        RunDish: RunDish,
        StopCooking: StopCooking,
        isMotorConnected: isMotorConnected
    };
});
