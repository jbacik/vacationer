import * as axios from 'axios';

// import { format } from 'date-fns';
// import { inputDateFormat } from './constants';

import { API } from './config';

const getUserInfo = async function () {
  try {
    //const response = await axios.get(`${API}/user.json`);
    const response = await axios.get(`http://localhost:5000/Users/GetUser`);
    let data = parseObject(response);
    return data;
  } catch (error) {
    console.error(error);
    return {};
  }
};

/* eslint-disable no-unused-vars */
const getHours = async function(userId) {
  // cant just return this, because its not what we want
  // return response.data;
  // but what if there is bad data in the response?
  // let data = response.data;
  // Let's parse it better
  
  try {
    const response = await axios.get(`${API}/hours.json`);
    
    let data = parseList(response);
    return data;
  } catch (error) {
    console.error(error);
    return [];
  }
};
/* eslint-enable no-unused-vars */

const parseList = response => {
  if (response.status !== 200) throw Error(response.message);
  if (!response.data) return [];
  let list = response.data;
  if (typeof list !== 'object') {
    list = [];
  }
  return list;
};

const parseObject = response => {
  if (response.status !== 200) throw Error(response.message);
  if (!response.data) return {};
  let obj = response.data;
  if (typeof obj !== 'object') {
    obj = {};
  }
  return obj;
};

export const data = {
  getHours, getUserInfo
};