export class ApiPreviewContentChangedEvent extends Event {
  static readonly TYPE = 'api-preview-content-changed';

  constructor() {
    super(ApiPreviewContentChangedEvent.TYPE);
  }
}
