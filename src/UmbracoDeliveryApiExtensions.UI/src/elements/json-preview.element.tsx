import JsonView from '@uiw/react-json-view';
import {type JsonViewProps} from '@uiw/react-json-view';

import defineReactElement from '../helpers/define-react-element';

const WebReactJsonComponent = (props: Pick<JsonViewProps<Record<string, unknown>>, 'value'>) =>
  <JsonView displayDataTypes={false} shortenTextAfterLength={50} collapsed={2} value={props.value}>
    <JsonView.Null render={(props, result) => result.type === 'value' ? <span {...props}>null</span> : <span/>}/>
    <JsonView.CountInfo render={(_props, result) => Array.isArray(result.value) ? undefined : <span/> }/>
  </JsonView>;

defineReactElement('bc-json-preview', WebReactJsonComponent, {props: {value: undefined}});
