import r2wc from '@r2wc/react-to-web-component';
import JsonView from '@uiw/react-json-view';
import {defineElement} from '@umbraco-ui/uui';

const WebReactJson = r2wc(JsonView, {
  props: {
    value: 'json',
    displayObjectSize: 'boolean',
    displayDataTypes: 'boolean',
    shortenTextAfterLength: 'number',
  },
});

defineElement('bc-json-preview')(WebReactJson);
