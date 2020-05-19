import axios from 'axios';
import { API_HOST } from '../../resources/config';

const bookService = {
    getBooks(model) {
        return axios.post(`${API_HOST}/api/Book/SearchBooks`, model);
    },
    deleteBook(id) {
        return axios.delete(`${API_HOST}/api/Book/DeleteBook/${id}`);
    },
    getBookDetail(id) {
        return axios.get(`${API_HOST}/api/Book/GetBook/${id}`);
    },
    updateBook(model) {
        return axios.put(`${API_HOST}/api/Book/UpdateBook`, model);
    },
    addBook(model) {
        return axios.post(`${API_HOST}/api/Book/AddBook`, model);
    },
    getAvailableBooks(model) {
        return axios.post(`${API_HOST}/api/AvailableBook/SearchAvailableBooks`, model);
    },
    deleteAvailableBook(id) {
        return axios.delete(`${API_HOST}​/api/AvailableBook/DeleteAvailableBook/${id}`);
    },
    addAvailableBook(model) {
        return axios.post(`${API_HOST}​/api/AvailableBook/AddAvailableBook`, model);
    },
    reservedBooksList(model) {
        return axios.post(`${API_HOST}/api/ReservedBook/SearchReservedBooks`, model);
    },
    returnReserverBook(id) {
        return axios.delete(`${API_HOST}/api/ReservedBook/ReturnBook/${id}`);
    },
    reserveBook(model) {
        return axios.post(`${API_HOST}/api/ReservedBook/ReserveBook`, model);
    }
}

export default bookService;