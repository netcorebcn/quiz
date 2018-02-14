function bindClass(classToBind) {
  Object.getOwnPropertyNames(classToBind.constructor.prototype)
    .filter(
      prop => typeof classToBind[prop] === 'function' && prop !== 'constructor'
    )
    .forEach(
      method => classToBind[method] = classToBind[method].bind(classToBind)
    );
}

const prettyPrint = (json) => JSON.stringify(json, undefined, 4);

export { bindClass, prettyPrint };
