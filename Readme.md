# Distributed Transaction with Saga Pattern Using MassTransit, Asp.net Core and RabbitMQ
distributed insert transaction between 3 databse with same model. for implement Saga Pattern used Masstransit Framework and Courier feature.
## 1-1 فرم تنظیمات
این فرم محلی برای تعیین انواع مختلف استعلامات و تنظیماتی است که توسط شهرداری یا توسط ادارات و دفاتر مختلف از شهرداری انجام می‌شود و شامل دو بخش **لیست** و **ثبت** است.
### 1-1-1 اقلام اطلاعاتی
| **ردیف** | **نام فیلد** | **نوع ورودی** | **محدودیت** | **توضیحات** |
| --- | --- | --- | --- | --- |
| **1** | موضوع | انتخابی | الزامی | از جدول موضوعات –نوع استعلام |
| **2** | مدت اعتبار پاسخ(روز) | عدد | | |
| **3** | مهلت پاسخ (روز) | عدد |
 |
 |
| **4** | عدم پاسخگویی به منزلۀ تأیید است؟ | انتخابی | | بله، خیر |
| **5** | تاریخ آخرین ویرایش | تاریخ | اجباری | از تاریخ فعلی سیستم دریافت می‌شود. |
| **6** | Zone | اجباری | | از جدول Zone |
**نکات اقلام اطلاعاتی**** :**
- **مدت اعتبار پاسخ**** :** برای استعلامات ورودی منظور از مدت اعتبار، مدت زمانیست که پاسخی که شهرداری ارسال می‌کند دارای اعتبار است و برای استعلامات خروجی، مدت اعتبار پاسخی که سایر ادارات به شهرداری ارسال می‌کنند مدنظر است.
- **مهلت پاسخ**** :** مدت زمانی که برای هر استعلام تعیین می‌شود تا پاسخی برای آن (چه از طرف شهرداری و چه سایر ادارات) تعیین شود.
- **نوع پاسخ**** :** به ازای هر موضوع شهرداری بایستی مشخص شود عدم پاسخگویی در مهلت مقرر ملزم به تأیید یا عدم تأیید است یا خیر؟.
