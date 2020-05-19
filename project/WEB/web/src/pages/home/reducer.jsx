import React from 'react';
import update from "../../redux/update";
import axios from 'axios';

import homeService from "./service";
import { openNotification, notificationTypes } from "../../resources/notifications";

const actions = {
    test: "HOME/test",
    setLoadingHomeValue: "Home/loading",
    setUserData: "Home/userData",
    setUserProfile: "Home/userProfile",
    setUserList: "Home/usersList"
}

const initialState = {
    loading: false,
    userData: JSON.parse(localStorage.getItem('user_data')),
    userProfile: {},
    setUserList: null
};

export const homeReducer = (state = initialState, action) => {
    let newState = state;

    switch (action.type) {
        case actions.test: {
            state = update.set(state, initialState);
            break;
        }
        case actions.setLoadingHomeValue: {
            newState = update.set(newState, 'loading', action.payload.value)
            break;
        }
        case actions.setUserData: {
            newState = update.set(newState, 'userData', action.payload.value)
            break;
        }
        case actions.setUserProfile: {
            newState = update.set(newState, 'userProfile', action.payload.value)
            break;
        }
        case actions.setUserList: {
            newState = update.set(newState, 'usersList', action.payload.value)
            break;
        }
        default: {
            axios.defaults.headers.common['authorization'] = `Bearer ${localStorage.getItem('access_token')}`;
            axios.defaults.headers.common['Pragma'] = 'no-cache';
            axios.interceptors.response.use(response => {
                return response;
            }, error => {
                if (error.response && error.response.status === 401) {
                    openNotification('Login', 'Please login', notificationTypes.WARNING)
                    return newState;
                }
                if (error.response && error.response.status === 403) {
                    openNotification('Restrict', 'You don`t have permissions for this action', notificationTypes.WARNING)
                    return newState;
                }
            })
            break;
        }
    }
    return newState;
}

export const login = model => {
    return (dispatch) => {
        dispatch(setLoadingHomeValueAction(true));

        homeService
            .login(model)
            .then(response => {
                if (response.data.isSuccessful) {
                    dispatch(setUserDataAction(response.data.data));
                    localStorage.setItem('access_token', response.data.data.authorizationToken)
                    localStorage.setItem('user_data', JSON.stringify(response.data.data))
                } else {
                    openNotification("Login failed", response.data.message, notificationTypes.ERROR)
                }
            })
            .catch(error => {
                console.log(error);
                openNotification("Login failed", "Some error. Please, try later", notificationTypes.ERROR)
            })
            .finally(() => {
                setTimeout(() => {
                    dispatch(setLoadingHomeValueAction(false));
                }, 0)
            })
    }
}

export const registration = model => {
    return (dispatch) => {
        dispatch(setLoadingHomeValueAction(true));

        homeService
            .registration(model)
            .then(response => {
                if (response.data.isSuccessful) {
                    openNotification('Registration was successful', null, notificationTypes.SUCCESS)
                } else {
                    openNotification("Registration failed", <div>{response.data.message.split(';').map((item, index) => <p style={{ marginBottom: '5px' }} key={index}>{item}</p>)}</div>, notificationTypes.ERROR)
                }
            })
            .catch(error => {
                console.log(error);
                openNotification("Registration failed", "Some error. At this moment we can't register you. Please, try later", notificationTypes.ERROR)
            })
            .finally(() => {
                setTimeout(() => {
                    dispatch(setLoadingHomeValueAction(false));
                }, 0)
            })
    }
}

export const isAuthorize = () => {
    const userData = JSON.parse(localStorage.getItem('user_data'));

    if (userData == null) {
        return true;
    } else {
        return homeService
            .isAuthorize({ id: userData.id, token: userData.authorizationToken })
            .then(response => {
                return response.data.isSuccessful;
            })
            .catch(error => {
                console.log(error);
                return false;
            })
    }
}

export const getUser = (userId) => {
    return (dispatch) => {
        dispatch(setLoadingHomeValueAction(true));

        return homeService
            .getUser(userId)
            .then(response => {
                if (!response.data.isSuccessful) {
                    openNotification('Error', response.data.message, notificationTypes.WARNING)
                } else {
                    dispatch(setUserProfileAction(response.data.data));
                }
            })
            .catch(error => {
                console.log(error);
                openNotification('Error', 'Some error. Can`t receive user profile details', notificationTypes.ERROR);
            })
            .finally(() => {
                setTimeout(() => {
                    dispatch(setLoadingHomeValueAction(false));
                }, 0)
            })
    }
}

export const updateProfile = model => {
    return (dispatch) => {
        dispatch(setLoadingHomeValueAction(true));

        return homeService
            .updateProfile(model)
            .then(response => {
                if (!response.data.isSuccessful) {
                    openNotification('Error', response.data.message, notificationTypes.WARNING)
                } else {
                    dispatch(setUserProfileAction(response.data.data));
                    openNotification('Saved changes', 'Profile details was successfully saved', notificationTypes.SUCCESS)
                }
            })
            .catch(error => {
                console.log(error);
                openNotification('Error', 'Some error. Can`t update user profile details', notificationTypes.ERROR);
            })
            .finally(() => {
                setTimeout(() => {
                    dispatch(setLoadingHomeValueAction(false));
                }, 0)
            })
    }
}

export const getUsersList = model => {
    return (dispatch) => {
        dispatch(setLoadingHomeValueAction(true));

        return homeService
            .getUsersList(model)
            .then(response => {
                if (!response.data.isSuccessful) {
                    openNotification('Error', response.data.message, notificationTypes.WARNING)
                } else {
                    dispatch(setUserListAction(response.data.items));
                }
            })
            .catch(error => {
                console.log(error);
                openNotification('Error', 'Some error. Can`t load users list', notificationTypes.ERROR);
            })
            .finally(() => {
                setTimeout(() => {
                    dispatch(setLoadingHomeValueAction(false));
                }, 0)
            })
    }
}

export const blockUser = (id, model) => {
    return (dispatch) => {
        dispatch(setLoadingHomeValueAction(true));

        return homeService
            .blockUser(id)
            .then(response => {
                if (!response.data.isSuccessful) {
                    openNotification('Error', response.data.message, notificationTypes.WARNING)
                } else {
                    openNotification('Success', 'User was blocked', notificationTypes.SUCCESS);
                    dispatch(getUsersList(model));
                }
            })
            .catch(error => {
                console.log(error);
                openNotification('Error', 'Some error. Can`t block user', notificationTypes.ERROR);
            })
            .finally(() => {
                setTimeout(() => {
                    dispatch(setLoadingHomeValueAction(false));
                }, 0)
            })
    }
}

export const unblockUser = (id, model) => {
    return (dispatch) => {
        dispatch(setLoadingHomeValueAction(true));

        return homeService
            .unblockUser(id)
            .then(response => {
                if (!response.data.isSuccessful) {
                    openNotification('Error', response.data.message, notificationTypes.WARNING)
                } else {
                    openNotification('Success', 'User was unblocked', notificationTypes.SUCCESS);
                    dispatch(getUsersList(model));
                }
            })
            .catch(error => {
                console.log(error);
                openNotification('Error', 'Some error. Can`t unblock user', notificationTypes.ERROR);
            })
            .finally(() => {
                setTimeout(() => {
                    dispatch(setLoadingHomeValueAction(false));
                }, 0)
            })
    }
}

export const logOut = () => {
    return (dispatch) => {
        dispatch(setUserDataAction(null));
        localStorage.clear();
    }
}

export const testAction = () => {
    return {
        type: actions.type,
        payload: {}
    }
}

export const setLoadingHomeValueAction = value => {
    return {
        type: actions.setLoadingHomeValue,
        payload: { value: value }
    }
}

export const setUserDataAction = value => {
    return {
        type: actions.setUserData,
        payload: { value: value }
    }
}

export const setUserProfileAction = value => {
    return {
        type: actions.setUserProfile,
        payload: { value: value }
    }
}

export const setUserListAction = value => {
    return {
        type: actions.setUserList,
        payload: { value: value }
    }
}