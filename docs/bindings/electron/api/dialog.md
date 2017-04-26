# dialog

## Methods

- [ ] `dialog.showOpenDialog([browserWindow, ]options[, callback])`
- [x] `dialog.showOpenDialog(options[, callback])`
* `options`
  - [x]  `title`
  - [x] `defaultPath`
  - [x] `buttonLabel`
  - [x]  `filters` [FileFilter[]]
  * `properties`
    - [x] `openFile`
    - [x]  `openDirectory` 
    - [x]  `multiSelections`
    - [x]  `showHiddenFiles`
    - [x]  `createDirectory`
    - [ ]  `promptToCreate`
    - [ ] `noResolveAliases`
  - [ ]  `message`
- [ ] `dialog.showSaveDialog([browserWindow, ]options[, callback])`
- [x] `dialog.showSaveDialog(options[, callback])`
* `options`
  - [x] `title` 
  - [x] `defaultPath`
  - [x] `buttonLabel`
  - [x] `filters` [FileFilter[]]
  - [x] `message`
  - [ ] `nameFieldLabel`
  - [ ] `showsTagField`
- [ ] `dialog.showMessageBox([browserWindow, ]options[, callback])`
- [x] `dialog.showMessageBox(options[, callback])`
* `options`
  - [x] `type`
  - [x] `buttons`
  - [x] `defaultId`
  - [x] `title`
  - [x] `message` String - Content of the message box.
  - [x] `detail`
  - [ ] `checkboxLabel`
  - [x] `checkboxChecked`
  - [x] `icon`
  - [x] `cancelId`
  - [x] `noLink`
  - [ ] `normalizeAccessKeys`

- [x] `dialog.showErrorBox(title, content)`
- [ ] `dialog.showCertificateTrustDialog([browserWindow, ]options, callback)` _macOS_
