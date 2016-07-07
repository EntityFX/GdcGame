angular
    .module("gdCameApp")
    .component("counters",
    {
        templateUrl: "/app/templates/counters.template.html",
        controller: function CountersController($rootScope, $scope) {
            $rootScope.$watch("gameData",
                function () {
                    if ($rootScope.gameData !== undefined)
                        $scope.counters = $rootScope.gameData.Counters;
                });
        }
    });
