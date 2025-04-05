const express = require('express');
const router = express.Router();
const LN_PicUpload_Module = require("../modules/LN_PicUpload_Module");
const e = require("cors");
require('dotenv').config(); // 載入環境變數


const multer = require('multer'); // 處理 multipart/form-data
const AWS = require('aws-sdk');
const { v4: uuidv4 } = require('uuid');



const storage = multer.memoryStorage(); // 設定 multer 使用記憶體儲存圖片
const upload = multer({ storage });

// 初始化 AWS S3 設定
const s3 = new AWS.S3({
    region: process.env.AWS_REGION,
    accessKeyId: process.env.AWS_ACCESS_KEY_ID,
    secretAccessKey: process.env.AWS_SECRET_ACCESS_KEY,
});

// 處理上傳圖片請求
router.route('/imageUpload').post(upload.single('image'), async (req, res) => {
    const file = req.file;
    const { email } = req.body;

    if (!file) return res.status(400).json({ error: 'No file uploaded' });

    // 產生唯一檔名並定義 S3 上傳參數
    const key = `profile/${uuidv4()}-${file.originalname}`;
    const params = {
        Bucket: process.env.AWS_BUCKET_NAME,
        Key: key,
        Body: file.buffer,
        ContentType: file.mimetype,
        ACL: 'private', // 設為 private，禁止公開讀取
    };

    try 
    {
        // 上傳到 S3
        const result = await s3.upload(params).promise();

        // 將使用者資訊儲存至 MongoDB
        const newPicUpload = new LN_PicUpload_Module({
        email,
        profileImageUrl: key,
        });
        await newPicUpload.save();

        res.json({ message: 'Uploaded and saved', user: newPicUpload });
    } 
    catch (err) 
    {
        console.error(err);
        res.status(500).json({ error: 'Upload failed' });
    }
});





router.route('/imageGet/by-email').get( async (req, res) => {
    //const email = req.params.email;
    const { email } = req.query;
    console.log(email);
  
    try {
      const records = await LN_PicUpload_Module.find({ email });
      const urls = [];
  
      for (let record of records) {
        const params = {
          Bucket: process.env.AWS_BUCKET_NAME,
          Key: record.profileImageUrl, // 這是 S3 上的 key
          Expires: 60 * 5, // 產生 5 分鐘有效的簽名 URL
        };
        
        console.log(record.profileImageUrl);
        const signedUrl = s3.getSignedUrl('getObject', params);
        urls.push(signedUrl);
      }
  
      res.json({ images: urls });
    } catch (err) {
      console.error(err);
      res.status(500).json({ error: '取得圖片失敗' , detail: err.message});
    }
});
  



module.exports = router;