import { getContentItemByPath } from './src/api/umbraco-api';

(async () => {
    let content = (await getContentItemByPath('/', { expand: "all" })).data;
    
    if(content.contentType === 'pageHome'){
        console.log(content.properties?.text);
    }
})();