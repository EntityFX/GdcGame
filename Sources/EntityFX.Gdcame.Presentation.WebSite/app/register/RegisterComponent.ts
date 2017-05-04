angular
    .module("gdCameApp")
    .component("register",
    {
        templateUrl: (viewTheme : string) =>
             "/app/views/" + viewTheme + "/registerView.html"
        ,
        controller: 'RegisterController',
        controllerAs: "controller"
    });