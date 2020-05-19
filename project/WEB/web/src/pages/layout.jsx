import React from 'react';
import { connect } from 'react-redux';
import { Layout, Menu } from 'antd';
import { Link } from 'react-router-dom';
import { logOut } from './home/reducer';

const { Header } = Layout;


class LayoutPageComponent extends React.Component {
    constructor(props) {
        super();

        this.state = {
            current: props.pathname,
        };
    }

    handleClick = e => {
        this.setState({
            current: e.key,
        });
    };

    render() {
        return (
            <div className="layout">
                <Header style={{ zIndex: 1, width: '100%', background: '#686058' }}>
                    <Menu mode="horizontal" defaultSelectedKeys={['1']} selectedKeys={[this.state.current]} onClick={this.handleClick} style={{ width: "100%", background: '#686058', height: 'auto' }}>
                        <Menu.Item key="1"><Link style={{ color: 'white' }} to="/books">List of Books</Link></Menu.Item>
                        {this.props.userDetails && this.props.userDetails.roles.includes('Admin') && <Menu.Item key="4"><Link style={{ color: 'white' }} to="/users">List of Users</Link></Menu.Item>}
                        <Menu.Item key="5"><Link style={{ color: 'white' }} to="/chat">Chat</Link></Menu.Item>
                        {this.props.userDetails && <Menu.Item key="3" style={{ float: 'right' }} onClick={e => this.props.logOut()}><Link style={{ color: 'white' }} to="/">Sign Out</Link></Menu.Item>}
                        {this.props.userDetails && <Menu.Item key="2" style={{ float: 'right' }}><Link style={{ color: 'white' }} to='/profile'>Welcome, <b>{this.props.userDetails.firstName} {this.props.userDetails.lastName}</b></Link></Menu.Item>}
                    </Menu>
                </Header>
                {this.props.children}
            </div>
        )
    }
}

const mapState = ({ home, routing }) => {
    return {
        userDetails: home.userData,
        pathname: routing.location ? routing.location.pathname : null
    }
};

const mapDispatch = (dispatch) => {
    return {
        logOut() {
            dispatch(logOut());
        }
    };
};

const LayoutPage = connect(mapState, mapDispatch)(LayoutPageComponent);
export default LayoutPage;