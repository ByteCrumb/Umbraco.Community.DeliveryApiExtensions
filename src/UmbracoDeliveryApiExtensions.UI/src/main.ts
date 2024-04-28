import {type UmbEntryPointOnInit} from '@umbraco-cms/backoffice/extension-api';
import {type ManifestWorkspaceView} from '@umbraco-cms/backoffice/extension-registry';

export const onInit: UmbEntryPointOnInit = (_host, extensionRegistry) => {
  const workspaceAlias = 'deliveryApiPreview';
  const apiPreviewManifest: ManifestWorkspaceView = {
    type: 'workspaceView',
    alias: workspaceAlias,
    name: 'Delivery API Preview',
    meta: {
      icon: 'icon-code',
      label: 'API',
      pathname: 'preview',
    },
    element: async () => (import('./workspace-views/api-preview')),
    weight: -50, // TODO: This should come from the app settings.
    conditions: [
      {
        alias: 'Umb.Condition.WorkspaceAlias',
        match: 'Umb.Workspace.Document',
      },
    ],
  };

  extensionRegistry.registerMany([apiPreviewManifest]);
};
