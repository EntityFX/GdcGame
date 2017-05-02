angular
    .module("gdCameApp")
    .component("login",
    {
        templateUrl: (viewTheme : string) =>
             "/app/views/" + viewTheme + "/loginView.html"
        ,
        controller: 'LoginController',
        controllerAs: "controller"
    });