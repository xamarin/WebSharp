/*---------------------------------------------------------
 * Copyright (C) Microsoft Corporation. All rights reserved.
 *--------------------------------------------------------*/
var nameRegex = /^[a-z0-9][a-z0-9\-]*$/;  // app name need to be lower case
var publisherRegex = /^[a-z0-9][a-z0-9\-]*$/i;

module.exports.validatePublisher = function(publisher) {
    if (!publisher) {
        return "Missing publisher name";
    }

    if (!publisherRegex.test(publisher)) {
        return "Invalid publisher name";
    }

    return true;
}

module.exports.validateAppId = function(id) {
    if (!id) {
        return "Missing extension identifier";
    }

    if (!nameRegex.test(id)) {
        return "Invalid extension identifier";
    }

    return true;
}