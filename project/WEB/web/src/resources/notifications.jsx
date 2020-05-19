//Antd
import { notification } from 'antd';

export const notificationTypes = {
    SUCCESS: 'success',
    INFO: 'info',
    ERROR: 'error',
    WARNING: 'warning'
}

export const openNotification = (title, message, type) => {
    notification[type]({
        message: title,
        description: message,
    });
};