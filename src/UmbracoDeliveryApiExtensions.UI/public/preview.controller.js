angular.module("umbraco")
    .controller("Umbraco.Community.DeliveryApiExtensions.Preview", function ($scope, $http, editorState, userService, contentResource) {
        var vm = this;

        const serverVariables = Umbraco.Sys.ServerVariables.deliveryApiPreview;
        const currentKey = editorState.current.key;

        if(editorState.current.udi.includes("media")){
          vm.ApiPath = serverVariables.mediaApiEndpoint;
        }else{
          vm.ApiPath = serverVariables.contentApiEndpoint;
        }

        vm.ApiPath = vm.ApiPath.replace("00000000-0000-0000-0000-000000000000", currentKey)
    });
