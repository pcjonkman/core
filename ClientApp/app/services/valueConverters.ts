export class NumberToStringValueConverter {
  toView(value: number) {
    // var _value = -1;
    // if (typeof(value) === 'string') {
    //   var stringToInt = parseInt(value);
    //   _value = isNaN(stringToInt) ? -1 : stringToInt;
    // } else {
    //   _value = isNaN(value) ? -1 : value;
    // }

    // return _value === -1 ? '' : `${_value}`;
    return value === -1 ? '' : `${value}`;
  }

  fromView(value: string) {
    // var _value = parseInt(value);
    // return isNaN(_value) ? this.toView(_value) : _value;
    return value;
  }
}