(function () {
    'use strict';

    window.app.controller('DashboardController', DashboardController);

    DashboardController.$inject = ['$window', '$http', '$scope'];
    function DashboardController($window, $http, $scope) {
        var vm = this;
        vm.errorMessage = null;
        vm.loggingIn = false;
        vm.DishesList = [];
        vm.RecipeList = [];
        vm.selectedVal = "";
        vm.hideAddDish = true;
        vm.hideDishList = true;
        vm.onAddDishClick = OnAddDish;
        vm.onCreateDish = createdish;
        vm.IngredientList = [];
        vm.saveDish = saveDish;
        vm.OnRecipeChange = recipeChange;

        $http({
            method: 'GET',
            url: '/Home/DishesList'
        }).then(function successCallback(response) {
            if (response.data.RecipeList.length > 0) {
                vm.hideDishList = false;
            }
            else {
                vm.hideAddDish = false;
            }
            console.log(response.data.RecipeList);
            vm.DishesList = response.data.RecipeList;
            vm.RecipeList = response.data.RecipeCollection;
            angular.element('#gr_hd_0').triggerHandler('click');
        }, function errorCallback(response) {
        });

        function OnAddDish(val) {
            if (val == "List") {
                $http({
                    method: 'GET',
                    url: '/Home/DishesList'
                }).then(function successCallback(response) {
                    vm.DishesList = response.data.RecipeList;
                }, function errorCallback(response) {
                });

                vm.hideDishList = false;
                vm.hideAddDish = true;
            }
            else {
                vm.selectedVal = "";
                vm.hideDishList = true;
                vm.hideAddDish = false;
                vm.IngredientList = [];
            }
        }

        function createdish() {
            if (vm.selectedVal > 0) {
                $http({
                    method: 'GET',
                    url: '/Home/GetIngredientList',
                    params: { recipeId: vm.selectedVal }
                }).then(function successCallback(response) {
                    console.log(response);
                    vm.IngredientList = response.data;
                }, function errorCallback(response) {

                });
            }
        }


        function saveDish(ingredientId, recipeId, checked) {
            console.log(recipeId);
            console.log(ingredientId);
            console.log(checked);

            $http({
                method: 'POST',
                url: '/Home/SaveDish',
                params: { recipeId: recipeId, ingredientId: ingredientId, check: checked }
            }).then(function successCallback(response) {
                console.log(response);
                // vm.IngredientList = response.data;
            }, function errorCallback(response) {

            });
        }

        function recipeChange()
        {
            vm.IngredientList = [];
        }


        $scope.oneAtATime = true;

        $scope.status = {
            isFirstOpen: true,
            isFirstDisabled: false
        };

    }
})();
