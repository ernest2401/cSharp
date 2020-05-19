import React from 'react'
import { Redirect, Route } from 'react-router-dom'
import { connect } from 'react-redux';

const PrivateRouteComponent = ({ component: Component, ...rest }) => {
    return (
        <Route
            {...rest}
            render={props =>
                rest.userData && (!rest.onlyAdmin || rest.onlyAdmin && rest.userData.roles.includes('Admin')) ? (
                    <Component {...props} />
                ) : (
                        <Redirect to={{ pathname: '404', state: { from: props.location } }} />
                    )
            }
        />
    )
}

const mapState = ({ home }) => {
    return {
        userData: home.userData
    }
};

const mapDispatch = (dispatch) => {
    return {}
};

const PrivateRoute = connect(mapState, mapDispatch)(PrivateRouteComponent);

export default PrivateRoute