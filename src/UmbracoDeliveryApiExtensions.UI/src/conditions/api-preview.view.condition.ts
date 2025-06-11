import { UmbControllerHost } from "@umbraco-cms/backoffice/controller-api";
import {
  ManifestCondition,
  UmbConditionConfigBase,
  UmbConditionControllerArguments,
  UmbExtensionCondition,
} from "@umbraco-cms/backoffice/extension-api";
import { UmbConditionBase } from "@umbraco-cms/backoffice/extension-registry";
import { UMB_DOCUMENT_WORKSPACE_CONTEXT } from '@umbraco-cms/backoffice/document';
import {UMB_MEDIA_WORKSPACE_CONTEXT} from "@umbraco-cms/backoffice/media";

export type ApiPreviewViewConfig = UmbConditionConfigBase;

export class ApiPreviewViewCondition
  extends UmbConditionBase<ApiPreviewViewConfig>
  implements UmbExtensionCondition
{
  constructor(
    host: UmbControllerHost,
    args: UmbConditionControllerArguments<ApiPreviewViewConfig>
  ) {
    super(host, args);

    this.consumeContext(UMB_DOCUMENT_WORKSPACE_CONTEXT, (context) => {
      if(!context) return;
      this.observe(context.isNew, (isNew) => {
        this.permitted = !isNew;
      });
    });

    this.consumeContext(UMB_MEDIA_WORKSPACE_CONTEXT, (context) => {
      if(!context) return;
      this.observe(context.isNew, (isNew) => {
        this.permitted = !isNew;
      });
    });
  }
}

export const manifest: ManifestCondition = {
  type: "condition",
  name: "API Preview View Condition",
  alias: "DeliveryApiExtensions.ApiPreview.View",
  api: ApiPreviewViewCondition,
};
