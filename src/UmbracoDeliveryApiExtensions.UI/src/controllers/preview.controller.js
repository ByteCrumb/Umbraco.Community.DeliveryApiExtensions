import angular from 'angular';

angular
  .module('umbraco')
  .controller(
    'Umbraco.Community.DeliveryApiExtensions.Preview',
    [
      '$scope',
      '$element',
      '$routeParams',
      'contentAppHelper',
      'eventsService',
      function ($scope, $element, $routeParams, contentAppHelper, eventsService) {
        const vm = this;

        registerAsKnownContentApp();

        vm.apiPath = $scope.model.viewModel.apiPath;
        vm.hasPreview = $scope.model.viewModel.entityType === 'document';
        vm.culture = $routeParams.cculture || $routeParams.mculture;

        updateIsPublished();
        const stateChangeWatch = $scope.$watch('variantContent.state', updateIsPublished);
        const contentSavedWatch = eventsService.on('content.saved', updatePreview);

        vm.$onDestroy = function () {
          stateChangeWatch();
          contentSavedWatch();
        };

        // Ensures the content save/publish are displayed
        function registerAsKnownContentApp() {
          const contentAppAlias = 'deliveryApiPreview';
          if (contentAppHelper.CONTENT_BASED_APPS.indexOf(contentAppAlias) === -1) {
            contentAppHelper.CONTENT_BASED_APPS.push(contentAppAlias);
          }
        }

        function updateIsPublished() {
          vm.isPublished = $scope.variantContent?.state !== 'Draft';
        }

        function updatePreview() {
          updateIsPublished();
          const apiPreviewElement = $element.find('bc-api-preview')[0];
          apiPreviewElement.updatePreviewResponse();
          apiPreviewElement.updatePublishedResponse();
        }
      },
    ],
  );
