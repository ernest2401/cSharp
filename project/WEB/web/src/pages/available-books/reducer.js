import update from "../../redux/update";
import bookService from "./service";
import { openNotification, notificationTypes } from "../../resources/notifications";

const actions = {
    setLoadingValue: "Books/setLoading",
    setBooksValue: "Books/setBooksList",
    setBookDetailsValue: "Books/setBookDetails",
    setAvailableBooks: "Books/setAvailableBooks",
    setReservedBooks: "Books/setReservedBooks"
}

const initialState = {
    loading: false,
    booksList: null,
    bookDetails: null,
    availableBooks: null,
    reservedBooks: null
};

export const booksReducer = (state = initialState, action) => {
    let newState = state;

    switch (action.type) {
        case actions.setLoadingValue: {
            newState = update.set(newState, 'loading', action.payload.value);
            break;
        }
        case actions.setBooksValue: {
            newState = update.set(newState, 'booksList', action.payload.value);
            break;
        }
        case actions.setBookDetailsValue: {
            newState = update.set(newState, 'bookDetails', action.payload.value);
            break;
        }
        case actions.setAvailableBooks: {
            newState = update.set(newState, 'availableBooks', action.payload.value);
            break;
        }
        case actions.setReservedBooks: {
            newState = update.set(newState, 'reservedBooks', action.payload.value);
        }
        default: {
            break;
        }
    }
    return newState;
}

export const getBooksList = (model) => {
    return (dispatch) => {
        dispatch(setLoadingValueAction(true));

        bookService
            .getBooks(model)
            .then(response => {
                if (response.data.isSuccessful) {
                    dispatch(setBooksValueAction(response.data.items))
                } else {
                    openNotification('Error', response.data.message, notificationTypes.WARNING);
                }
            })
            .catch(error => {
                console.log(error);
                openNotification('Error', 'Some error. Can`t load list of books', notificationTypes.ERROR);
            })
            .finally(() => {
                setTimeout(() => {
                    dispatch(setLoadingValueAction(false));
                }, 0)
            })
    }
}

export const deleteBook = (id, model) => {
    return (dispatch) => {
        dispatch(setLoadingValueAction(true));

        bookService
            .deleteBook(id)
            .then(response => {
                if (response.data.isSuccessful) {
                    openNotification('Deleted', 'Book was successfully deleted', notificationTypes.SUCCESS);
                    dispatch(getBooksList(model));
                } else {
                    openNotification('Deleted', response.data.message, notificationTypes.WARNING);
                }
            })
            .catch(error => {
                console.log(error);
                openNotification('Error', 'Some error. Can`t delete book', notificationTypes.ERROR);
            })
            .finally(() => {
                setTimeout(() => {
                    dispatch(setLoadingValueAction(false));
                }, 0)
            })
    }
}

export const addAvailableBook = (model) => {
    return (dispatch) => {
        dispatch(setLoadingValueAction(true));

        bookService
            .addAvailableBook(model)
            .then(response => {
                if (response.data.isSuccessful) {
                    openNotification('Success', 'Available book was successfully added', notificationTypes.SUCCESS);
                } else {
                    openNotification('Error', response.data.message, notificationTypes.WARNING);
                }
            })
            .catch(error => {
                console.log(error);
                openNotification('Error', 'Some error. Can`t add available book', notificationTypes.ERROR);
            })
            .finally(() => {
                setTimeout(() => {
                    dispatch(setLoadingValueAction(false));
                }, 0)
            })
    }
}

export const getBooksDetails = id => {
    return (dispatch) => {
        dispatch(setLoadingValueAction(true));

        bookService
            .getBookDetail(id)
            .then(response => {
                if (response.data.isSuccessful) {
                    dispatch(setBookDetailsValueAction(response.data.data))
                } else {
                    openNotification('Error', response.data.message, notificationTypes.WARNING);
                }
            })
            .catch(error => {
                console.log(error);
                openNotification('Error', 'Some error. Can`t load book details', notificationTypes.ERROR);
            })
            .finally(() => {
                setTimeout(() => {
                    dispatch(setLoadingValueAction(false));
                }, 0)
            })
    }
}

export const updateBooksDetails = model => {
    return (dispatch) => {
        dispatch(setLoadingValueAction(true));

        bookService
            .updateBook(model)
            .then(response => {
                if (response.data.isSuccessful) {
                    openNotification('Saved', 'Changes successfully saved', notificationTypes.SUCCESS);
                    dispatch(setBookDetailsValueAction(response.data.data))
                } else {
                    openNotification('Error', response.data.message, notificationTypes.WARNING);
                }
            })
            .catch(error => {
                console.log(error);
                openNotification('Error', 'Some error. Can`t save changes of book details', notificationTypes.ERROR);
            })
            .finally(() => {
                setTimeout(() => {
                    dispatch(setLoadingValueAction(false));
                }, 0)
            })
    }
}

export const createBook = model => {
    return (dispatch) => {
        dispatch(setLoadingValueAction(true));

        bookService
            .addBook(model)
            .then(response => {
                if (response.data.isSuccessful) {
                    openNotification("Book created", "Book was successfully created", notificationTypes.SUCCESS);
                    window.location = '/books';
                } else {
                    openNotification('Error', response.data.message, notificationTypes.WARNING);
                }
            })
            .catch(error => {
                console.log(error);
                openNotification('Error', 'Some error. Can`t create book', notificationTypes.ERROR);
            })
            .finally(() => {
                setTimeout(() => {
                    dispatch(setLoadingValueAction(false));
                }, 0)
            })
    }
}

export const getAvailableBooksList = (model) => {
    return (dispatch) => {
        dispatch(setLoadingValueAction(true));

        bookService
            .getAvailableBooks(model)
            .then(response => {
                if (response.data.isSuccessful) {
                    dispatch(setAvailableBooksAction(response.data.items))
                } else {
                    openNotification('Error', response.data.message, notificationTypes.WARNING);
                }
            })
            .catch(error => {
                console.log(error);
                openNotification('Error', 'Some error. Can`t load list of available books', notificationTypes.ERROR);
            })
            .finally(() => {
                setTimeout(() => {
                    dispatch(setLoadingValueAction(false));
                }, 0)
            })
    }
}

export const deleteAvailableBook = (id, model) => {
    return (dispatch) => {
        dispatch(setLoadingValueAction(true));

        bookService
            .deleteAvailableBook(id)
            .then(response => {
                if (response.data.isSuccessful) {
                    openNotification('Deleted', 'Available book was successfully deleted', notificationTypes.SUCCESS);
                    dispatch(getAvailableBooksList(model));
                } else {
                    openNotification('Error', response.data.message, notificationTypes.WARNING);
                }
            })
            .catch(error => {
                console.log(error);
                openNotification('Error', 'Some error. Can`t delete available book', notificationTypes.ERROR);
            })
            .finally(() => {
                setTimeout(() => {
                    dispatch(setLoadingValueAction(false));
                }, 0)
            })
    }
}

export const getReservedBooks = (model) => {
    return (dispatch) => {
        dispatch(setLoadingValueAction(true));

        bookService
            .reservedBooksList(model)
            .then(response => {
                if (response.data.isSuccessful) {
                    dispatch(setReservedBooksAction(response.data.items));
                } else {
                    openNotification('Error', response.data.message, notificationTypes.WARNING);
                }
            })
            .catch(error => {
                console.log(error);
                openNotification('Error', 'Some error. Can`t receive reserved books', notificationTypes.ERROR);
            })
            .finally(() => {
                setTimeout(() => {
                    dispatch(setLoadingValueAction(false));
                }, 0)
            })
    }
}

export const returnBook = (id, model) => {
    return (dispatch) => {
        dispatch(setLoadingValueAction(true));

        bookService
            .returnReserverBook(id)
            .then(response => {
                if (response.data.isSuccessful) {
                    openNotification('Success', 'Book was returned', notificationTypes.SUCCESS);
                    dispatch(getReservedBooks(model));
                    dispatch(getAvailableBooksList(model));
                } else {
                    openNotification('Error', response.data.message, notificationTypes.WARNING);
                }
            })
            .catch(error => {
                console.log(error);
                openNotification('Error', 'Some error. Can`t return books', notificationTypes.ERROR);
            })
            .finally(() => {
                setTimeout(() => {
                    dispatch(setLoadingValueAction(false));
                }, 0)
            })
    }
}

export const reserveBook = (model, filter) => {
    return (dispatch) => {
        dispatch(setLoadingValueAction(true));

        bookService
            .reserveBook(model)
            .then(response => {
                if (response.data.isSuccessful) {
                    openNotification('Success', 'Book was reserved', notificationTypes.SUCCESS);
                    dispatch(getReservedBooks(filter));
                    dispatch(getAvailableBooksList(filter));
                } else {
                    openNotification('Error', response.data.message, notificationTypes.WARNING);
                }
            })
            .catch(error => {
                console.log(error);
                openNotification('Error', 'Some error. Can`t reserve book', notificationTypes.ERROR);
            })
            .finally(() => {
                setTimeout(() => {
                    dispatch(setLoadingValueAction(false));
                }, 0)
            })
    }
}

export const setLoadingValueAction = value => {
    return {
        type: actions.setLoadingValue,
        payload: { value }
    }
}

export const setBooksValueAction = value => {
    return {
        type: actions.setBooksValue,
        payload: { value }
    }
}

export const setBookDetailsValueAction = value => {
    return {
        type: actions.setBookDetailsValue,
        payload: { value }
    }
}

export const setAvailableBooksAction = value => {
    return {
        type: actions.setAvailableBooks,
        payload: { value }
    }
}

export const setReservedBooksAction = value => {
    return {
        type: actions.setReservedBooks,
        payload: { value }
    }
}