angular
    .module("gdCameApp")
    .component("menu",
    {
        templateUrl: function (viewTheme) {
            return "/app/views/"+viewTheme+"/gameMenuView.html";
        },
        controller: 'MenuController',
        controllerAs: "controller"
    });