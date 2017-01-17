angular
    .module("gdCameApp")
    .component("gameMenu",
    {
        templateUrl: function (viewTheme) {
            return "/app/views/"+viewTheme+"/gameMenuView.html";
        },
        controller: 'GameMenuController'
    });