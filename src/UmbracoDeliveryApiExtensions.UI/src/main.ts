import {type UmbEntryPointOnInit} from '@umbraco-cms/backoffice/extension-api';

import {manifest as apiPreviewViewCondition} from './conditions/api-preview.view.condition';
import {ApiPreviewRepository} from './contexts/api-preview.repository';

export const onInit: UmbEntryPointOnInit = (_host, extensionRegistry) => {
  const workspaceAlias = 'deliveryApiPreview';
  const apiPreviewRepository = new ApiPreviewRepository(_host);

  apiPreviewRepository
    .fetchConfig()
    .then(config => {
      if (config?.enabled !== true) {
        return;
      }

      extensionRegistry.register(apiPreviewViewCondition);
      const enabledWorkspaces = ['Umb.Workspace.Document'];
      if (config.media.enabled) {
        enabledWorkspaces.push('Umb.Workspace.Media');
      }

      const apiPreviewManifest: UmbExtensionManifest = {
        type: 'workspaceView',
        alias: workspaceAlias,
        name: 'Delivery API Preview',
        meta: {
          icon: 'icon-code',
          label: 'API',
          pathname: 'preview',
        },
        element: async () => (import('./workspace-views/api-preview')),
        weight: 110,
        conditions: [
          {
            alias: 'Umb.Condition.WorkspaceAlias',
            oneOf: enabledWorkspaces,
          },
          {
            alias: 'DeliveryApiExtensions.ApiPreview.View',
          },
        ],
      };

      extensionRegistry.register(apiPreviewManifest);
    });
};
