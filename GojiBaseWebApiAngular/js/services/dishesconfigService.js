App.factory("dishesconfigService", function ($http) {


    function getAllDishes()
    {
        return $http.get("/api/dishesconfig/getAllDishes");
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
        var url = "/api/dishesconfig/CreateNewDish";
        return $http.post("/api/dishesconfig/CreateNewDish", dish);
    }

    function UpdateDish(dish) {
        var url = "/api/dishesconfig/UpdateDish";
        return $http.post("/api/dishesconfig/UpdateDish", dish);
    }

    return {
        getAllDishes: getAllDishes,
        getDishById: getDishById,
        deleteDishById: deleteDishById,
        CreateNewDish: CreateNewDish,
        UpdateDish: UpdateDish
    };

});