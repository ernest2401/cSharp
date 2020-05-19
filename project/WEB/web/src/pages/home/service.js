import axios from 'axios';
import { API_HOST } from '../../resources/config';

const homeService = {
    registration(model) {
        return axios.post(`${API_HOST}/api/Account/Register`, model);
    },
    login(model) {
        return axios.post(`${API_HOST}/api/Account/Login`, model);
    },
    isAuthorize(model) {
        return axios.post(`${API_HOST}/api/Account/Authorize`, model);
    },
    getUser(id) {
        return axios.get(`${API_HOST}/api/Account/GetUser/${id}`);
    },
    updateProfile(model) {
        return axios.put(`${API_HOST}/api/Account/UpdateUser`, model);
    },
    getUsersList(model) {
        return axios.post(`${API_HOST}/api/Account/SearchUsers`, model);
    },
    blockUser(id) {
        return axios.post(`${API_HOST}/api/Account/BlockUser/${id}`);
    },
    unblockUser(id) {
        return axios.post(`${API_HOST}/api/Account/UnBlockUser/${id}`);
    }
}

export default homeService;