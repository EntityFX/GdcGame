angular.module("gdCameApp").controller("ItemsController",
[
    "$rootScope", "$scope", "gameData", "gdCameApiService",
    function ($rootScope, $scope, gameData, gdCameApiService) {
        $scope.items = [];
        $scope.itemsGroups = [];

        gameData.then(function (data) {
            $scope.items = data.items;
            var indexInGroup = 0;
            var itemsGroup = [];
            $scope.items.forEach(function (value, index) {
                if (indexInGroup === 4) {
                    $scope.itemsGroups.push(itemsGroup);
                    itemsGroup = [];
                    indexInGroup = 0;
                }
                itemsGroup.push(value);
                indexInGroup++;
            });
            if (itemsGroup.length > 0) {
                itemsGroup.push(new Array(4 - itemsGroup.length));
                $scope.itemsGroups.push(itemsGroup);
            }
        });

        $scope.$on("update.items",
            function (event, value) {
                $scope.items = value.items;
            });

        $scope.buyFundDriver = function (fundDriverId) {
            var oldFundDriver = $scope.items.filter(function (item, index) {
                return item.id === fundDriverId;
            })[0];

            if (oldFundDriver && oldFundDriver.unlockBalance > $rootScope.gameData.cash.counters[0].value) {
                return;
            }

            gdCameApiService.buyFundDriver(fundDriverId)
                .then(function (value) {
                    if (value.data == undefined) return;
                    var itemIndex;
                    var oldFundDriver = $scope.items.filter(function (item, index) {
                        itemIndex = index;
                        return item.id === value.data.itemBuyInfo.id;
                    })[0];

                    if (oldFundDriver != undefined) {
                        oldFundDriver.bought = value.data.itemBuyInfo.bought;
                        oldFundDriver.price = value.data.itemBuyInfo.price;

                        if (value.data.modifiedCountersInfo != undefined) {
                            $rootScope.$broadcast("counters.update", value.data.modifiedCountersInfo);
                        }
                    }
                });
        };
    }
]);