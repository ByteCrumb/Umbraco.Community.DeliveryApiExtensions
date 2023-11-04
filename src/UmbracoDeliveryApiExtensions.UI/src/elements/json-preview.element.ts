import JsonView from '@uiw/react-json-view';
import {type FunctionComponent} from 'preact';
import register from 'preact-custom-element';

register(JsonView as FunctionComponent, 'bc-json-preview', ['value', 'display-object-size', 'display-data-types', 'shorten-text-after-length'], {shadow: true});
