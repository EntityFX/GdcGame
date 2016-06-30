angular
    .module("gdCameApp")
    .component('counters',
    {
        templateUrl: '/app/templates/counters.template.html',
        controller: function CountersController($rootScope, $scope, gdCameApiService) {
            $rootScope.$watch('gameData.Counters',
                function () {
                    if ($rootScope.gameData !== undefined)
                        $scope.counters = $rootScope.gameData.Counters;
                });
        }
    });
