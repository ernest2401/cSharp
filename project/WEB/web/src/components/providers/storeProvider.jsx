//React
import React from 'react';

//Redux
import { Provider } from 'react-redux';
import { store } from '../../redux/store';

/* Connect application to the store */
const withStoreProvider = ComposedComponent => {
    return props => {
        return (
            <Provider store={store}>
                <ComposedComponent {...props} />
            </Provider >
        )
    }
}

export default withStoreProvider;