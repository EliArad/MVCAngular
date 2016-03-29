App.factory("dishesconfigService", function ($http) {


    function getAllDishes()
    {
        return $http.get("/api/dishesconfig/getAllDishes");
    }

    function GetAppConfig() {
        return $http.get("/api/AppConfig/GetAppConfig");
    } 

    function SetDishImageId(id) {

        var url = "/api/dishesconfig/SetDishImageId";
        return $http({
            url: url,
            method: "GET",
            params: { id: id }
        });
    }

    function getDishById(id) {

        var url = "/api/dishesconfig/getDishById";
        return $http({
            url: url,
            method: "GET",
            params: { id: id }
        });
    }

    function deleteDishById(id) {
        var url = "/api/dishesconfig/deleteDishById";
        return $http({
            url: url,
            method: "GET",
            params: { id: id }
        });
    }

    function CreateNewDish(dish) {
        return $http.post("/api/dishesconfig/CreateNewDish", dish);
    }

    function UpdateDish(dish) {
        return $http.post("/api/dishesconfig/UpdateDish", dish);
    }

    function UpdateAppConfig(config) {
        return $http.post("/api/AppConfig/SaveAppConfig", config);
    }

    return {
        getAllDishes: getAllDishes,
        getDishById: getDishById,
        deleteDishById: deleteDishById,
        CreateNewDish: CreateNewDish,
        UpdateDish: UpdateDish,
        SetDishImageId: SetDishImageId,
        UpdateAppConfig: UpdateAppConfig,
        GetAppConfig: GetAppConfig

    };

});