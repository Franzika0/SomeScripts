const mongoose = require('mongoose');

const LN_PicUpload_Schema = new mongoose.Schema({
  email: String,
  profileImageUrl: String,
});

const LN_PicUpload_Module = mongoose.model("LN_PicUpload_Module", LN_PicUpload_Schema);

module.exports = LN_PicUpload_Module;
