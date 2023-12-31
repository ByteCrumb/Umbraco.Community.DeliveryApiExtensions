import createClient from 'openapi-fetch';

import {type components, type paths} from './api/umbraco-api';

(async () => {
  const {GET} = createClient<paths>({baseUrl: 'http://localhost:34962'});

  const {data} = await GET('/umbraco/delivery/api/v2/content/item/{path}', {
    params: {
      query: {
        expand: 'properties[$all]',
      },
      path: {
        path: '/',
      },
    },
  });

  if (data) {
    renderPage(data);
  }
})();

function renderPage(content: components['schemas']['IApiContentResponseModel']) {
  console.log('  Name: ', content.name);
  console.log('  Path: ', content.route?.path);

  if (content.contentType === 'testPage') {
    renderTestPage(content);
  }
}

function renderTestPage(content: components['schemas']['TestPageContentResponseModel']) {
  const {properties} = content;

  console.log('\n  **Common**');
  print('textString', properties?.textString);
  print('textArea', properties?.textArea);
  print('datePickerWithTime', properties?.datePickerWithTime);
  print('datePicker', properties?.datePicker);
  print('toggle', properties?.toggle);
  print('numeric', properties?.numeric);
  print('decimal', properties?.decimal);
  print('slider', properties?.slider);
  print('tags', properties?.tags);
  print('email', properties?.email);

  console.log('\n  **Pickers**');
  print('colorPicker', properties?.colorPicker);
  print('contentPicker', '<tested below>');
  print('eyeDropperColorPicker', properties?.eyeDropperColorPicker);
  print('urlPicker', properties?.urlPicker);
  print('multinodeTreepicker', '<tested below>');

  console.log('\n  **Rich content**');
  print('richText', properties?.richText);
  print('blockGrid', '<tested below>');
  print('markdown', properties?.markdown);

  console.log('\n  **People**');
  print('memberGroupPicker', properties?.memberGroupPicker);
  print('memberPicker', properties?.memberPicker);
  print('userPicker', properties?.userPicker);

  console.log('\n  **Lists**');
  print('blockList', '<tested below>');
  print('checkboxList', properties?.checkboxList);
  print('dropdown', properties?.dropdown);
  print('radiobox', properties?.radiobox);
  print('repeatableTextstrings', properties?.repeatableTextstrings);

  console.log('\n  **Media**');
  print('uploadFile', properties?.uploadFile);
  print('imageCropper', properties?.imageCropper);
  print('mediaPicker', properties?.mediaPicker);

  console.log('\n  **Content Picker**');
  print('name', properties?.contentPicker?.name);
  print('route>path', properties?.contentPicker?.route?.path);
  print('properties>textString', properties?.contentPicker?.properties?.textString);

  console.log('\n  **Multinode Treepicker**');
  print('name', properties?.multinodeTreepicker?.[0]?.name);
  print('route>path', properties?.multinodeTreepicker?.[0]?.route?.path);
  print('properties>textString', properties?.multinodeTreepicker?.[0]?.properties?.textString);

  console.log('\n  **Block List**');
  content.properties?.blockList?.items?.forEach((block, i) => {
    console.log(`    Block[${i}]:`);
    renderBlock(block);
  });

  console.log('\n  **Block Grid**');
  content.properties?.blockGrid?.items?.forEach((block, i) => {
    console.log(`    Block[${i}]:`);
    renderBlock(block);
  });

  console.log('\n  **From composition(s)**');
  print('  sharedToggle', properties?.sharedToggle);
  print('  sharedString', properties?.sharedString);
  print('  sharedRadiobox', properties?.sharedRadiobox);
  print('  sharedRichText', properties?.sharedRichText);
}

function renderBlock(block: components['schemas']['ApiBlockItemModel']) {
  console.log('      Type: ', block.content?.contentType);
  switch (block.content?.contentType) {
    case 'testBlock': {
      console.log('      String: ', block.content.properties?.string);
      console.log('      Multinode Treepicker: ', block.content.properties?.multinodeTreepicker?.[0]?.id);
      console.log('      Shared string: ', block.content.properties?.sharedString);
      const nestedBlock = block.content.properties?.blocks?.items?.[0];
      if (nestedBlock) {
        console.log('      **Nested block**');
        renderBlock(nestedBlock);
      }

      break;
    }

    case 'testBlock2': {
      console.log('      Shared string (testBlock2): ', block.content.properties?.sharedString);
      if (block.settings?.contentType === 'blockSettings') {
        console.log('      Block id (settings): ', block.settings?.properties?.anchorId);
      }

      break;
    }

    default:
      console.error('      Unknown block type	');
      break;
  }
}

function print(propertyName: string, value: unknown) {
  console.log(`    ${propertyName} (${typeof value}):`, JSON.stringify(value));
}
