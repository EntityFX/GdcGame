angular
    .module("gdCameApp")
    .component('fundDrivers',
    {
        templateUrl: '/app/templates/fundDrivers.template.html',
        controller: function FundDriversController($rootScope, $scope, gameData, gdCameApiService) {
            gameData.then(function (data) {
                $scope.fundsDrivers = data.FundsDrivers;
            });

            $.connection.hub.start();
            $scope.buyFundDriver = function(fundDriverId) {
                gdCameApiService.buyFundDriver(fundDriverId);
                gdCameApiService.getGameData().then(function (value) {
                    $scope.fundsDrivers = value.data.FundsDrivers;
                });
            }
        }
    });
