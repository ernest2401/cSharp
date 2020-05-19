//React
import React from 'react';
import { connect } from 'react-redux';

//Antd
import { Input, Form, Spin, Select, DatePicker, Button, Row, Col } from 'antd';

//Other
import LayoutPage from '../layout';
import moment from 'moment';
import './index.scss';
import { getUser, updateProfile, setUserProfileAction } from '../home/reducer';

//Constants
const { Option } = Select;

class ProfilePageComponent extends React.Component {
    state = {}

    componentDidMount() {
        const userData = JSON.parse(localStorage.getItem('user_data'));
        this.props.getUser(this.props.userId ? this.props.userId : userData.id);
    }

    componentWillUnmount() {
        this.props.setEmptyProfile();
    }

    updateProfile = values => {
        this.props.updateProfile({ ...values, id: this.props.userProfile.id });
    }

    render() {
        let { userProfile } = this.props;
        userProfile.birthday = userProfile.birthday ? moment(userProfile.birthday) : userProfile.birthday;

        return (
            <LayoutPage>
                <div className="profile-page">
                    <h1>Profile</h1>
                    <Spin spinning={this.props.loading}>
                        {
                            userProfile.id &&
                            <Form name="profile-form" onFinish={this.updateProfile}
                                initialValues={userProfile}
                                className="profile-form">
                                <Row>
                                    <Col xs={24} sm={24} md={12} lg={12} xl={12} className="left-panel">
                                        <Form.Item name="firstName" rules={[
                                            {
                                                required: true,
                                                message: 'Please input your First Name',
                                            },
                                        ]}>
                                            <Input id="firstName" placeholder="First Name" />
                                        </Form.Item>
                                        <Form.Item name="lastName" rules={[
                                            {
                                                required: true,
                                                message: 'Please input your Last Name',
                                            },
                                        ]}>
                                            <Input id="secondName" placeholder="Last Name" />
                                        </Form.Item>
                                        <Form.Item name="email" rules={[
                                            {
                                                type: 'email',
                                                message: 'The input is not valid Email',
                                            },
                                            {
                                                required: true,
                                                message: 'Please input your Email',
                                            },
                                        ]}>
                                            <Input readOnly id="email" placeholder="Email" />
                                        </Form.Item>
                                    </Col>
                                    <Col xs={24} sm={24} md={12} lg={12} xl={12} className="left-panel">
                                        <Form.Item name="phoneNumber" rules={[
                                            {
                                                required: true,
                                                message: 'Please input your Phone Number',
                                            },
                                        ]}>
                                            <Input id="phoneNumber" placeholder="Phone Number" />
                                        </Form.Item>
                                        <Form.Item className="text-left" name="sex" rules={[{ required: true, message: 'Please choose your gender' }]}>
                                            <Select placeholder="Gender">
                                                <Option value={1}>Male</Option>
                                                <Option value={2}>Female</Option>
                                            </Select>
                                        </Form.Item>
                                        <Form.Item name="birthday" rules={[
                                            {
                                                required: true,
                                                message: 'Please input your Birthday Date',
                                            },
                                        ]}>
                                            <DatePicker className="full-width" />
                                        </Form.Item>
                                    </Col>
                                </Row>
                                <Button className="registration-btn" htmlType="submit">Save Changes</Button>
                            </Form>
                        }
                    </Spin>
                </div>
            </LayoutPage >
        )
    }
}

const mapState = ({ home }) => {
    return {
        userProfile: home.userProfile || {},
        loading: home.loading
    }
};

const mapDispatch = (dispatch) => {
    return {
        getUser(id) {
            dispatch(getUser(id));
        },
        updateProfile(model) {
            dispatch(updateProfile(model))
        },
        setEmptyProfile() {
            dispatch(setUserProfileAction(null))
        }
    };
};

const ProfilePage = connect(mapState, mapDispatch)(ProfilePageComponent);
export default ProfilePage;