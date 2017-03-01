(function()
{
    let v8Util = process.atomBinding('v8_util');

    const websharpObjectCache = v8Util.createIDWeakMap();
    let websharpObjId = 0;

    var RegisterScriptableObject = function (meta)
    {
        let id = v8Util.getHiddenValue(meta, 'websharpId');
        if (id) {
            websharpObjectCache.set(id, meta);
        }
        else
        {
            id = websharpObjId++;
            v8Util.setHiddenValue(meta, 'websharpId', id);
            websharpObjectCache.set(id, meta);
        }
        return id;

    }

    var GetScriptableObject = function (id) {
        //console.log('get scriptobject with ' + id);
        return websharpObjectCache.get(id);

    }

    var UnRegisterScriptableObject = function (key) {

        console.log('key ' + key );

    }

    module.exports = { RegisterScriptableObject, GetScriptableObject, UnRegisterScriptableObject };

})();