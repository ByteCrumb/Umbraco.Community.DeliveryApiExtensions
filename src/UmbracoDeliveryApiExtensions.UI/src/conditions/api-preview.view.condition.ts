import { UmbControllerHost } from "@umbraco-cms/backoffice/controller-api";
import {
  ManifestCondition,
  UmbConditionConfigBase,
  UmbConditionControllerArguments,
  UmbExtensionCondition,
} from "@umbraco-cms/backoffice/extension-api";
import { UmbConditionBase } from "@umbraco-cms/backoffice/extension-registry";

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

    // TODO: Check if the document or media item is "previewable"
    this.permitted = true;
    args.onChange();
  }
}

export const manifest: ManifestCondition = {
  type: "condition",
  name: "API Preview View Condition",
  alias: "DeliveryApiExtensions.ApiPreview.View",
  api: ApiPreviewViewCondition,
};
