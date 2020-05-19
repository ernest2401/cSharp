//React
import React from 'react';
import { connect } from 'react-redux';

//Antd
import { Row, Col, Table, Spin, Input, Select, Button, Modal } from 'antd';

//Other
import LayoutPage from '../layout';
import './index.scss';
import { Link } from 'react-router-dom';
import { BookGenre, BookCondition, UserStatus } from '../../resources/constants';
import { getUsersList, blockUser, unblockUser } from '../home/reducer';

//Constants
const { Option } = Select;
const { confirm } = Modal;

class UsersListPageComponent extends React.Component {
    state = {}

    componentDidMount() {
        this.props.getUsersList({});
    }

    componentWillUnmount() {
        this.setState({ status: null });
    }

    blockUser = id => {
        this.props.blockUser(id, this.state);
    }

    unblockUser = id => {
        this.props.unblockUser(id, this.state);
    }

    confirmBlock = id => {
        confirm({
            title: 'Are you really want to block user?',
            onOk: () => this.blockUser(id)
        });
    }

    confirmUnBlock = id => {
        confirm({
            title: 'Are you really want to unblock user?',
            onOk: () => this.unblockUser(id)
        });
    }

    getColumns = () => {
        return [
            {
                title: 'First Name',
                dataIndex: 'firstName',
                render: (text, item) => <Link to={`/user/${item.id}`}>{text}</Link>
            },
            {
                title: 'Last Name',
                dataIndex: 'lastName',
                render: (text, item) => <Link to={`/user/${item.id}`}>{text}</Link>
            },
            {
                title: 'Email',
                dataIndex: 'email'
            },
            {
                title: 'Phone Number',
                dataIndex: 'phoneNumber'
            },
            {
                title: 'Status',
                dataIndex: 'status',
                render: text => UserStatus.find(item => item.key == text).text
            },
            {
                render: item => item.status == 1 ?
                    <Button type='danger' onClick={e => this.confirmBlock(item.id)} style={{ width: '100px' }} ghost>Block</Button> : <Button onClick={e => this.confirmUnBlock(item.id)} style={{ width: '100px' }} type='primary' ghost>Unblock</Button>
            }
        ];
    }

    render() {
        const statuses = UserStatus.map(item => <Option key={item.key} value={item.key}>{item.text}</Option>);

        return (
            <LayoutPage>
                <Spin spinning={this.props.loading} >
                    <div className="users-list-page">
                        <Row>
                            <Col xs={24} sm={24} md={17} lg={17} xl={17}>
                                <h1>Users</h1>
                            </Col>
                        </Row>
                        <Row>
                            <Col xs={24} sm={24} md={17} lg={17} xl={17} className="left-panel">
                                {
                                    this.props.usersList &&
                                    <Table columns={this.getColumns()} dataSource={this.props.usersList} />
                                }
                            </Col>
                            <Col xs={24} sm={24} md={7} lg={7} xl={7} className="right-panel">
                                <h2>Filter</h2>
                                <Select allowClear className="full-width input-margin" placeholder="Status" onChange={e => this.setState({ status: e })}>
                                    {statuses}
                                </Select>
                                <Button className="full-width input-margin" onClick={e => this.props.getUsersList(this.state)}>Filter</Button>
                            </Col>
                        </Row>
                    </div>
                </Spin>
            </LayoutPage>
        )
    }
}

const mapState = ({ home }) => {
    return {
        loading: home.loading,
        usersList: home.usersList
    }
};

const mapDispatch = (dispatch) => {
    return {
        getUsersList(model) {
            dispatch(getUsersList(model));
        },
        blockUser(id, model) {
            dispatch(blockUser(id, model));
        },
        unblockUser(id, model) {
            dispatch(unblockUser(id, model));
        }
    };
};

const UsersListPage = connect(mapState, mapDispatch)(UsersListPageComponent);
export default UsersListPage;