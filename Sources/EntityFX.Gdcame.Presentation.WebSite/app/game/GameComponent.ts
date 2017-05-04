angular
    .module("gdCameApp")
    .component("game",
    {
        templateUrl: function (viewTheme) {
            return "/app/views/"+viewTheme+"/gameView.html";
        },
        controller: 'GameController',
        controllerAs: "controller"
    });